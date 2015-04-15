using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.DAL
{
    public interface IRepository
    {
        User FindUserById(int id);
        void AddNewFriend(int newFriendId);
        void RemoveFriend(int friendId);
        bool IsFriend(int newFriendId);
        List<UserProfile> GetAllFriends(int userId);
        List<UserProfile>  GetAllOnlineFriends(int userId);
        List<Dialog> GetAllDialogs();
        List<WallPost> GetAllNews();
        Dialog FindDialogWithUser(int userId);
        void AddNewMessage(Dialog currentDialog, string message);
        void AddNewPost(string post);
        void DeleteWallPost(WallPost post);
        void ChangeRating(int postId, int sizeOfChange);
        List<WallPost> GetAllPosts(int userId);
        void AddPhoto(Photo newPhoto);
        Photo GetAvatar(int userId);
        void SaveFields(User user);
        List<UserProfile> SearchUsers(string searchString);
        void AddComment(int wallPostId, string comment);
        Photo GetPhotoById(int photoId);
        List<Message> GetNewMessages(int lastMessageId);
        void UpdateLastVisit();
    }
}
