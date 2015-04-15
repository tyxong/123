using Microsoft.Web.WebPages.OAuth;
using SocialNet.Helpers;
using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace SocialNet.DAL
{
    public class Repository : IRepository
    {
        private readonly IHelperForStringFriends helperForFriends;

        public Repository(IHelperForStringFriends helper)
        {
            helperForFriends = helper;
        }

        public User FindUserById(int id)
        {
            using (var db = new Context())
            {
                return db.Users.Include("WallPosts").Include("Photos").FirstOrDefault(p => p.UserId == id);
            }
        }

        public void AddNewFriend(int newFriendId)
        {
            using (var db = new Context())
            {
                string stringOfFriends = db.Users.Find(WebSecurity.CurrentUserId).Friends;
                db.Users.Find(WebSecurity.CurrentUserId).Friends = helperForFriends.AddNewFriendToString(newFriendId, stringOfFriends);
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public void RemoveFriend(int friendId)
        {
            using (var db = new Context())
            {
                string stringOfFriends = db.Users.Find(WebSecurity.CurrentUserId).Friends;
                db.Users.Find(WebSecurity.CurrentUserId).Friends = helperForFriends.RemoveFriendToString(friendId, stringOfFriends);
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public bool IsFriend(int newFriendId)
        {
            using (var db = new Context())
            {
                if (helperForFriends.IsContain(newFriendId, db.Users.Find(WebSecurity.CurrentUserId).Friends))
                {
                    return true;
                }
                return false;
            }
        }

        public List<UserProfile> GetAllFriends(int userId)
        {
            using (var db = new Context())
            {
                List<User> allFriends = new List<User>();
                List<int> allId = helperForFriends.GetAllFriendsIdFromString(db.Users.Find(userId).Friends);
                foreach (var id in allId)
                {
                    allFriends.Add(db.Users.Include("WallPosts").FirstOrDefault(u => u.UserId == id));
                }
                List<UserProfile> result = new List<UserProfile>();
                foreach (var friend in allFriends)
                {
                    UserProfile userForView = new UserProfile
                    {
                        UserId = friend.UserId,
                        UserName = friend.UserName,
                        CurrentCity = friend.CurrentCity,
                        Mobile = friend.Mobile,
                        WallPosts = friend.WallPosts,
                        LastVisit = friend.LastVisit
                    };
                    userForView.IsFriend = IsFriend(userForView.UserId);
                    userForView.IsOnline = false;
                    if (userForView.LastVisit.AddMinutes(15) > DateTime.Now)
                    {
                        userForView.IsOnline = true;
                    }
                    result.Add(userForView);
                }
                UpdateLastVisit();
                return result;
            }
        }

        public List<Dialog> GetAllDialogs()
        {
            using (var db = new Context())
            {
                User currentUser = db.Users.Find(WebSecurity.CurrentUserId);
                var allDialogs = db.Dialogs.Include("Users").Include("Messages").ToList();
                UpdateLastVisit();
                return allDialogs.Where(m => (m.Users.Contains(currentUser))&&(m.Messages.Count != 0)).ToList();
            }
        }

        public Dialog FindDialogWithUser(int userId)
        {
            using (var db = new Context())
            {
                User userToWrite = db.Users.Include("Dialogs").FirstOrDefault(s => s.UserId == userId);
                User currentUser = db.Users.Include("Dialogs").FirstOrDefault(s => s.UserId == WebSecurity.CurrentUserId);
                Dialog dialogWithThisUser = currentUser.Dialogs.FirstOrDefault(m => m.Users.Contains(userToWrite));

                if (dialogWithThisUser == null)
                {
                    Dialog newDialog = new Dialog { Users = new List<User> { userToWrite, currentUser } };
                    db.Dialogs.Add(newDialog);
                    db.SaveChanges();
                    dialogWithThisUser = currentUser.Dialogs.FirstOrDefault(m => m.Users.Contains(userToWrite));
                }
                UpdateLastVisit();
                return db.Dialogs.Include("Messages").FirstOrDefault(d => d.DialogId == dialogWithThisUser.DialogId); ;
            }
        }

        public void AddNewMessage(Dialog currentDialog, string message)
        {
            using (var db = new Context())
            {
                Message newMessage = new Message
                {
                    UserId = WebSecurity.CurrentUserId,
                    Author = db.Users.Find(WebSecurity.CurrentUserId),
                    Text = message,
                    Time = DateTime.Now,
                    DialogId = currentDialog.DialogId,
                    Dialog = db.Dialogs.Find(currentDialog.DialogId)
                };
                db.Dialogs.Find(currentDialog.DialogId).Messages.Add(newMessage);
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public void AddNewPost(string post)
        {
            using (var db = new Context())
            {
                WallPost newPost = new WallPost
                {
                    Date = DateTime.Now,
                    Rating = 0,
                    Text = post,
                    UserId = WebSecurity.CurrentUserId,
                    User = db.Users.Find(WebSecurity.CurrentUserId)
                };
                db.Wallposts.Add(newPost);
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public void ChangeRating(int postId,int sizeOfChange)
        {
            using (var db = new Context())
            {
                WallPost postToChangeRating = db.Wallposts.Find(postId);
                postToChangeRating.Rating += sizeOfChange;
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public List<WallPost> GetAllPosts(int userId)
        {
            using (var db = new Context())
            {
                List<WallPost> result = new List<WallPost>();
                List<WallPost> allPosts = db.Users.Include("WallPosts").FirstOrDefault(u => u.UserId == userId).WallPosts.ToList();
                foreach(var post in allPosts)
                {
                    db.Entry(post).Collection("Comments").Load();
                    foreach(var comment in post.Comments)
                    {
                        db.Entry(comment).Reference("User").Load();
                    }
                    result.Add(post);
                }
                UpdateLastVisit();
                result.Reverse();
                return result; 
            }
        }

        public List<WallPost> GetAllNews()
        {
            using (var db = new Context())
            {
                List<UserProfile> friends = GetAllFriends(WebSecurity.CurrentUserId);
                List<WallPost> news = new List<WallPost>();
                foreach(var friend in friends)
                {                  
                    news = news.Concat(GetAllPosts(friend.UserId)).ToList();
                }
                news.Sort((n1, n2) => DateTime.Compare(n2.Date, n1.Date));
                UpdateLastVisit();
                return news;
            }
        }

        public void AddPhoto(Photo newPhoto)
        {
            using (var db = new Context())
            {
                List<Photo> allPhotosThisUser = db.Users.Include("Photos").FirstOrDefault(u => u.UserId == WebSecurity.CurrentUserId).Photos.ToList();
                if (newPhoto.IsAvatar)
                {
                    foreach (var photo in allPhotosThisUser)
                    {
                        photo.IsAvatar = false;
                    }
                }
                newPhoto.User = db.Users.Include("Photos").FirstOrDefault(u => u.UserId == WebSecurity.CurrentUserId);
                newPhoto.UserId = WebSecurity.CurrentUserId;
                db.Photos.Add(newPhoto);
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public Photo GetAvatar(int userId)
        {
            using (var db = new Context())
            {
                User user = db.Users.Include("Photos").FirstOrDefault(u => u.UserId == userId);
                return user.Photos.FirstOrDefault(ph => ph.IsAvatar == true);
            }
        }

        public void SaveFields(User user)
        {
            using (var db = new Context())
            {
                User userFromDB = db.Users.Find(WebSecurity.CurrentUserId);
                userFromDB.CurrentCity = user.CurrentCity;
                userFromDB.Adress = user.Adress;
                userFromDB.RelationShip = user.RelationShip;
                userFromDB.Mobile = user.Mobile;
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public List<UserProfile> SearchUsers(string searchString)
        {
            using (var db = new Context())
            {
                List<User> allUsers = db.Users.Where(u => u.UserName.Contains(searchString)).ToList();
                List<UserProfile> result = new List<UserProfile>();
                foreach (var friend in allUsers)
                {
                    UserProfile userForView = new UserProfile
                    {
                        UserId = friend.UserId,
                        UserName = friend.UserName,
                        CurrentCity = friend.CurrentCity,
                        Mobile = friend.Mobile,
                        LastVisit = friend.LastVisit
                    };
                    userForView.IsFriend = IsFriend(userForView.UserId);
                    userForView.IsOnline = false;
                    if (userForView.LastVisit.AddMinutes(15) > DateTime.Now)
                    {
                        userForView.IsOnline = true;
                    }
                    result.Add(userForView);
                }
                UpdateLastVisit();
                return result;
            }
        }

        public void AddComment(int wallPostId, string comment)
        {
            using (var db = new Context())
            {
                WallPost currentPost = db.Wallposts.Include("Comments").FirstOrDefault(w => w.WallPostId == wallPostId);
                Comment newComment = new Comment{
                    DateOfComment = DateTime.Now,
                    TextOfComment = comment,
                    UserId = WebSecurity.CurrentUserId,
                    User = db.Users.Find(WebSecurity.CurrentUserId),
                    WallPostId = wallPostId,
                    WallPost = currentPost
                };
                currentPost.Comments.Add(newComment);
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public Photo GetPhotoById(int photoId)
        {
            using (var db = new Context())
            {
                return db.Photos.Find(photoId);
            }
        }

        public void DeleteWallPost(WallPost post)
        {
            using (var db = new Context())
            {
                WallPost postToDelete = db.Wallposts.Find(post.WallPostId);
                db.Wallposts.Remove(postToDelete);
                db.SaveChanges();
            }
            UpdateLastVisit();
        }

        public List<Message> GetNewMessages(int lastMessageId)
        {
            using (var db = new Context())
            {
                if (lastMessageId < 0)
                {
                    Dialog dialog = db.Dialogs.Include("Messages").FirstOrDefault(d => d.DialogId == lastMessageId * -1);
                    List<Message> messages = dialog.Messages.ToList();
                    foreach (var message in messages)
                    {
                        db.Entry(message).Reference("Author").Load();
                    }
                    return messages;
                }
                Message lastMessage = db.Messages.Find(lastMessageId); 
                Dialog currentDialog = db.Dialogs.Include("Messages").FirstOrDefault(d => d.DialogId == lastMessage.DialogId);
                if (currentDialog.Messages.OrderByDescending(m => m.MessageId).FirstOrDefault().MessageId == lastMessageId)
                {
                    return new List<Message>();
                }
                Message lastAdded = currentDialog.Messages.First(m => m.MessageId == lastMessageId);
                List<Message> newMessages = currentDialog.Messages.Skip(currentDialog.Messages.OrderBy(m => m.MessageId).ToList().IndexOf(lastAdded) + 1).ToList();
                foreach (var message in newMessages)
                {
                    db.Entry(message).Reference("Author").Load();
                }
                return newMessages;
            }
        }

        public void UpdateLastVisit()
        {
            using (var db = new Context())
            {
                User currentUser = db.Users.Find(WebSecurity.CurrentUserId);
                currentUser.LastVisit = DateTime.Now;
                db.SaveChanges();
            }
        }

        public List<UserProfile> GetAllOnlineFriends(int userId)
        {
            using (var db = new Context())
            {
                List<User> allFriends = new List<User>();
                List<int> allId = helperForFriends.GetAllFriendsIdFromString(db.Users.Find(userId).Friends);
                foreach (var id in allId)
                {
                    User userForView = db.Users.Include("WallPosts").FirstOrDefault(u => u.UserId == id);
                    if (userForView.LastVisit.AddMinutes(15) > DateTime.Now)
                    {
                        allFriends.Add(userForView);
                    }
                }
                List<UserProfile> result = new List<UserProfile>();
                foreach (var friend in allFriends)
                {
                    UserProfile userForView = new UserProfile
                    {
                        UserId = friend.UserId,
                        UserName = friend.UserName,
                        CurrentCity = friend.CurrentCity,
                        Mobile = friend.Mobile,
                        WallPosts = friend.WallPosts,
                        LastVisit = friend.LastVisit
                    };
                    userForView.IsFriend = IsFriend(userForView.UserId);
                    userForView.IsOnline = false;
                    if (userForView.LastVisit.AddMinutes(15) > DateTime.Now)
                    {
                        userForView.IsOnline = true;
                    }
                    result.Add(userForView);
                }
                return result;
            }
        }
    }
}