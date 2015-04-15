using SocialNet.DAL;
using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace SocialNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository repository;

        public HomeController(IRepository newRepository)
        {
            repository = newRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            User foundUser = repository.FindUserById(WebSecurity.CurrentUserId);
            return View(CreateUserForView(foundUser));
        }

        [HttpGet]
        public ActionResult AllMyDialogs()
        {
            return View(repository.GetAllDialogs());
        }

        [HttpGet]
        public ActionResult ViewFriends(int? userId)
        {
            if (userId == null)
            {
                ViewBag.Title = "Мои Друзья";
                return View("ViewUsers", repository.GetAllFriends(WebSecurity.CurrentUserId));
            }
            ViewBag.Title = "Друзья " + repository.FindUserById((int)userId).UserName;
            return View("ViewUsers", repository.GetAllFriends((int)userId));
        }

        [HttpGet]
        public ActionResult ViewOnlineFriends(int? userId)
        {
            if (userId == null)
            {
                ViewBag.Title = "Мои Друзья " + repository.FindUserById((int)userId).UserName;
                return View("ViewUsers", repository.GetAllOnlineFriends(WebSecurity.CurrentUserId));
            }
            ViewBag.Title = "Друзья " + repository.FindUserById((int)userId).UserName;
            return View("ViewUsers", repository.GetAllOnlineFriends((int)userId));
        }

        [HttpGet]
        public ActionResult ViewPhotos(int? photoId)
        {
            if (photoId == null)
            {
                ViewBag.PhotoToView = -1;
                return View(repository.FindUserById(WebSecurity.CurrentUserId).Photos);
            }
            Photo photo = repository.GetPhotoById((int)photoId);
            ViewBag.PhotoToView = photo.PhotoId;
            return View(repository.FindUserById(photo.UserId).Photos);
        }

        [HttpGet]
        public ActionResult UserProfile(User user)
        {
            if (user.UserId == WebSecurity.CurrentUserId)
            {
                return RedirectToAction("Index");
            }
            User foundUser = repository.FindUserById(user.UserId);
            return View(CreateUserForView(foundUser));
        }

        [HttpGet]
        public ActionResult AddFriend(int newFriendId)
        {
            repository.AddNewFriend(newFriendId);
            return RedirectToAction("UserProfile",repository.FindUserById(newFriendId));
        }

        [HttpGet]
        public ActionResult RemoveFriend(int friendId)
        {
            repository.RemoveFriend(friendId);
            return RedirectToAction("UserProfile",repository.FindUserById(friendId));
        }


        [HttpGet]
        public ActionResult ViewDialog(int userId)
        {
            ViewBag.Title = "Диалог с пользователем " + repository.FindUserById(userId).UserName;
            return View(repository.FindDialogWithUser(userId));
        }

        [HttpPost]
        public void WriteMessage(Dialog dialog,string message)
        {
            if (message != null)
            {
                repository.AddNewMessage(dialog, message);
            }
        }

        [HttpPost]
        public ActionResult WriteWallPost(string post)
        {
            if (post != null)
            {
                repository.AddNewPost(post);
            }
            return RedirectToAction("RenderWall", repository.FindUserById(WebSecurity.CurrentUserId));
        }

        [HttpGet]
        public ActionResult DeleteWallPost(WallPost post)
        {
            repository.DeleteWallPost(post);
            return PartialView();
        }

        [HttpGet]
        public ActionResult IncRating(WallPost post)
        {
            HttpCookie voteCookie = Request.Cookies["Votes"];
            if (voteCookie != null)
            {
                if (voteCookie[post.WallPostId.ToString()] != null)
                {
                    return PartialView(post);
                }
            }
            else
            {
                voteCookie = new HttpCookie("Votes");
            }
            voteCookie[post.WallPostId.ToString()] = "Инк";
            Response.Cookies.Add(voteCookie);      
      
            repository.ChangeRating(post.WallPostId, 1);
            post.Rating++;
            return PartialView(post);
        }

        [HttpGet]
        public ActionResult DecRating(WallPost post)
        {
            HttpCookie voteCookie = Request.Cookies["Votes"];
            if (voteCookie != null)
            {
                if (voteCookie[post.WallPostId.ToString()] != null)
                {
                    return PartialView(post);
                }
            }
            else
            {
                voteCookie = new HttpCookie("Votes");
            }
            voteCookie[post.WallPostId.ToString()] = "Инк";
            Response.Cookies.Add(voteCookie);
            repository.ChangeRating(post.WallPostId, -1);
            post.Rating--;
            return PartialView(post);
        }

        [HttpGet]
        public ActionResult AddNewPhoto()
        {
            Photo newPhoto = new Photo();
            return View(newPhoto);
        }

        [HttpPost]
        public ActionResult AddNewPhoto(Photo newPhoto, HttpPostedFileBase image)
        {
            if (image != null)
            {
                newPhoto.ImageMimeType = image.ContentType;
                newPhoto.Picture = new byte[image.ContentLength];
                image.InputStream.Read(newPhoto.Picture, 0, image.ContentLength);
                repository.AddPhoto(newPhoto);
            }
            return RedirectToAction("Index");
        }

        public FileContentResult GetAvatar(int userId)
        {
            Photo avatar = repository.GetAvatar(userId);
            if (avatar != null)
            {
                return File(avatar.Picture, avatar.ImageMimeType);
            }
            else
            {
                var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Images"), "camera_200.png");
                return File(System.IO.File.ReadAllBytes(path), "image/png");
            }
        }

        public FileContentResult GetPhoto(int photoId)
        {
            Photo photo = repository.GetPhotoById(photoId);
            return File(photo.Picture, photo.ImageMimeType);
        }

        [HttpPost]
        public ActionResult Search(string searchString)
        {
            ViewBag.Title = "Поиск " + searchString;
            return View("ViewUsers",repository.SearchUsers(searchString));
        }

        [HttpPost]
        public ActionResult AddComment(WallPost post,string comment)
        {
            if (comment != null)
            {
                repository.AddComment(post.WallPostId, comment);
            }
            return PartialView(repository.GetAllPosts(post.UserId).FirstOrDefault(p => p.WallPostId == post.WallPostId).Comments.LastOrDefault());
        }

        [HttpGet]
        public ActionResult AllMyNews()
        {
            List<WallPost> allPosts = repository.GetAllNews();
            if (allPosts.Count > 10)
            {
                ViewBag.NotAllPosts = true;
                return View(allPosts.Take(10).ToList());
            }
            ViewBag.NotAllPosts = false;
            return View(allPosts);
        }

        [HttpGet]
        public ActionResult ViewMoreNews(WallPost lastAddedWallPost)
        {
            List<WallPost> allPosts = repository.GetAllNews();
            WallPost lastAdded = allPosts.First(m => m.WallPostId == lastAddedWallPost.WallPostId);
            List<WallPost> newPostsToView = allPosts.Skip(allPosts.IndexOf(lastAdded) + 1).ToList();
            if (newPostsToView.Count > 10)
            {
                ViewBag.NotAllPosts = true;
                return PartialView(allPosts.Take(11 + allPosts.IndexOf(lastAdded)).ToList());
            }
            ViewBag.NotAllPosts = false;
            return PartialView(allPosts);
        }
        
        public ActionResult RenderWall(User user)
        {
            List<WallPost> allPosts = repository.GetAllPosts(user.UserId);
            if (allPosts.Count > 10)
            {
                ViewBag.NotAllPosts = true;
                return PartialView(allPosts.Take(10).ToList());
            }
            ViewBag.NotAllPosts = false;
            return PartialView(allPosts);
        }
        
        [HttpGet]
        public ActionResult ViewMorePosts(WallPost lastAddedWallPost)
        {
            List<WallPost> allPosts = repository.GetAllPosts(lastAddedWallPost.UserId);
            WallPost lastAdded = allPosts.First(m => m.WallPostId == lastAddedWallPost.WallPostId);
            List<WallPost> newPostsToView = allPosts.Skip(allPosts.IndexOf(lastAdded)+1).ToList();
            if (newPostsToView.Count > 10)
            {
                ViewBag.NotAllPosts = true;
                return PartialView("RenderWall", allPosts.Take(11 + allPosts.IndexOf(lastAdded)).ToList());
            }
            ViewBag.NotAllPosts = false;
            return PartialView("RenderWall",allPosts);
        }

        [HttpGet]
        public ActionResult AddNewMessages(int lastMessageId)
        {
            return PartialView(repository.GetNewMessages(lastMessageId));
        }

        private UserProfile CreateUserForView(User foundUser)
        {
            UserProfile userForView = new UserProfile 
            { 
                UserId = foundUser.UserId,
                UserName = foundUser.UserName,
                WallPosts = foundUser.WallPosts,
                Photos = foundUser.Photos,
                Adress = foundUser.Adress,
                CurrentCity = foundUser.CurrentCity,
                Mobile = foundUser.Mobile,
                RelationShip = foundUser.RelationShip,
                LastVisit = foundUser.LastVisit
            };
            userForView.IsOnline = false;
            if (userForView.LastVisit.AddMinutes(15) > DateTime.Now)
            {
                userForView.IsOnline = true;
            }
            userForView.IsFriend = repository.IsFriend(userForView.UserId);
            userForView.Friends = repository.GetAllFriends(userForView.UserId);
            userForView.OnlineFriends = repository.GetAllOnlineFriends(userForView.UserId);
            return userForView;
        }
    }
}
