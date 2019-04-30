using nwBlog.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.WebApp.Areas.Admin.ViewModels.Home;
using nwBlog.WebApp.Helpers;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Result;
using nwBlog.WebApp.Models;
using System.Web.Helpers;
using nwBlog.WebApp.Models.Notification;
using nwBlog.WebApp.Filters;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class HomeController : Controller
    {
        private BlogManager _blogManager;
        private CategoryManager _categoryManager;
        private UserRoleManager _userRoleManager;
        private AppUserManager _userManager;
        private LogManager _logManager;
        private CommentManager _commentManager;
        private LikeManager _likeManager;

      

        public HomeController()
        {
            _blogManager = new BlogManager();
            _categoryManager = new CategoryManager();
            _userRoleManager = new UserRoleManager();
            _userManager = new AppUserManager();
            _logManager = new LogManager();
            _commentManager = new CommentManager();
            _likeManager = new LikeManager();
        }

        
        // GET: Admin/Home
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            model.BlogsCount = _blogManager.List().Count();
            model.CategoriesCount = _categoryManager.List().Count();
            model.UsersCount = _userRoleManager.GetUsersCount();
            model.AuthorUserCount = _userRoleManager.GetAuthorUserCount();

            model.BlogsLast5 = _blogManager.ListQueryable().OrderByDescending(x => x.CreatedOn).Take(5).ToList();


            return View(model);
        }      

        public ActionResult MyInfo()
        {           
            MyInfoViewModel model = new MyInfoViewModel();
            model.User = _userManager.Find(x => x.Id == CurrentSession.User.Id);

            return View(model);
        }

        public ActionResult EditMyInfo()
        {
            
            EditMyInfoViewModel model = new EditMyInfoViewModel();
            model.User = _userManager.Find(x => x.Id == CurrentSession.User.Id);

            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.User.CityId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMyInfo(EditMyInfoViewModel model)
        {
            ModelState.Remove("User.ModifiedOn");
            ModelState.Remove("User.ModifiedUsername");
            ModelState.Remove("User.Password");
            ModelState.Remove("User.ProfileImageFilename");
            ModelState.Remove("User.ActivateGuid");
            ModelState.Remove("User.IsDeleted");
            ModelState.Remove("User.IsActive");
            if (ModelState.IsValid)
            {
                AppUser UpdateUser = model.User;            

                BusinessLayerResult<AppUser> res = _userManager.UpdateFromAdmin(UpdateUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.User.CityId);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("MyInfo");
                }

            }


            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.User.CityId);
            return View(model);
        }

        public ActionResult ChangeMyPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Find(x => x.Id == CurrentSession.User.Id);
                if (Crypto.VerifyHashedPassword(user.Password, model.Password))
                {
                    user.Password = model.NewPassword;

                    BusinessLayerResult<AppUser> res = _userManager.ChangePasswordFromUser(user);
                    if (res.Errors.Count > 0)
                    {
                        res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                        return View(model);
                    }
                    else
                    {
                        OkViewModel notifyObj = new OkViewModel()
                        {
                            Title = "İşlem Başarılı",
                            RedirectingUrl = Url.Action("MyInfo", "Home")
                        };

                        notifyObj.Items.Add("Şifreniz değiştirilmiştir.");
                        return View("Ok", notifyObj);
                    }

                }

            }

            return View(model);
        }
        
        public ActionResult MyBlogs()
        {
            MyBlogsViewModel model = new MyBlogsViewModel();
            model.Blogs = _blogManager.ListQueryable().Where(x => x.AppUserId == CurrentSession.User.Id).ToList();

            return View(model);
        }

        public ActionResult MyLogs()
        {     
            MyLogsViewModel model = new MyLogsViewModel();
            model.Logs = _logManager.ListQueryable().Where(x => x.Username == CurrentSession.User.Username).ToList();
            return View(model);
        }

        public ActionResult MyLikes()
        {       
            MyLikesViewModel model = new MyLikesViewModel();
            model.Likes = _likeManager.ListQueryable().Where(x => x.AppUserId == CurrentSession.User.Id).ToList();
            return View(model);
        }

        public ActionResult MyComments()
        {        

            MyCommnentsViewModel model = new MyCommnentsViewModel();
            model.Comments = _commentManager.ListQueryable().Where(x => x.AppUserId == CurrentSession.User.Id).ToList();
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });         
        }

    }
}