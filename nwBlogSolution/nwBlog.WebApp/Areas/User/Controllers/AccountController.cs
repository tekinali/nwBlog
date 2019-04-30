using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using nwBlog.BusinessLayer;
using nwBlog.BusinessLayer.Result;
using nwBlog.Entities;
using nwBlog.WebApp.Areas.User.ViewModels.Account;
using nwBlog.WebApp.Filters;
using nwBlog.WebApp.Helpers;
using nwBlog.WebApp.Models;
using nwBlog.WebApp.Models.Notification;

namespace nwBlog.WebApp.Areas.User.Controllers
{
    [Auth]
    [AuthUser]
    [ActFilter]
    [Exc]
    public class AccountController : Controller
    {
        AppUserManager _userManager;
        LikeManager _likeManager;
        CommentManager _commentManager;


        public AccountController()
        {
            _userManager = new AppUserManager();
            _likeManager = new LikeManager();
            _commentManager = new CommentManager();
        }


        // GET: User/Account
        public ActionResult MyLikes()
        {
            var likes = _likeManager.ListQueryable().Where(x => x.AppUserId == CurrentSession.User.Id).ToList();
      
            return View(likes);
        }

        public ActionResult MyComments()
        {
            var commnets = _commentManager.ListQueryable().Where(x => x.AppUserId == CurrentSession.User.Id).ToList();

            return View(commnets);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CreateCommentViewModel model)
        {
            if(ModelState.IsValid)
            {
                Comment comment = new Comment()
                {
                    AppUserId=CurrentSession.User.Id,
                    BlogId=model.BlogId,
                    Text=model.Text.Trim()
                };
                int res = _commentManager.Insert(comment);
            }


            return RedirectToAction("BlogPost", "Home", new { @Blog = model.BlogId });
        }

        public ActionResult DeleteComment(int? id)
        {
            var comment = _commentManager.Find(x => x.Id == id && x.AppUserId == CurrentSession.User.Id);
            if(comment==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int res = _commentManager.Delete(comment);

            return RedirectToAction("MyComments");
        }

        public ActionResult MyInfo()
        {
            MyInfoViewModel model = new MyInfoViewModel();
            model.User = _userManager.Find(x => x.Id == CurrentSession.User.Id);

            return View(model);
        }

        public ActionResult MyInfoEdit()
        {
            var user = _userManager.Find(x => x.Id == CurrentSession.User.Id);

            MyInfoEditViewModel model = new MyInfoEditViewModel();
            model.About = user.About;
            model.CityId = user.CityId;
            model.Email = user.Email;
            model.Name = user.Name;
            model.Surname = user.Surname;

            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyInfoEdit(MyInfoEditViewModel model)
        {
            if(ModelState.IsValid)
            {
                AppUser UpdateUser = _userManager.Find(x => x.Id == CurrentSession.User.Id);

                UpdateUser.Name = model.Name;
                UpdateUser.Surname = model.Surname;
                UpdateUser.About = model.About;
                UpdateUser.Email = model.Email;
                UpdateUser.CityId = model.CityId;

                BusinessLayerResult<AppUser> res = _userManager.UpdateFromUser(UpdateUser);
                if(res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("MyInfo");
                }

            }

            return View(model);
        }

        public ActionResult DeleteAccount()
        {
            var user = _userManager.Find(x => x.Id == CurrentSession.User.Id);
            BusinessLayerResult<AppUser> res = _userManager.DeleteFromUser(user);
            if (res.Errors.Count > 0)
            {

            }
            OkViewModel notifyObj = new OkViewModel()
            {
                Title = "İşlem Başarılı",
                RedirectingUrl = Url.Action("Index", "Home", new { area = "" })
            };

            notifyObj.Items.Add("Üyeliğiniz silinmiştir.");
            return View("Ok", notifyObj);        
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = _userManager.Find(x => x.Id == CurrentSession.User.Id);
                if(Crypto.VerifyHashedPassword(user.Password,model.Password))
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
                            RedirectingUrl = Url.Action("MyInfo", "Account")
                        };

                        notifyObj.Items.Add("Şifreniz değiştirilmiştir.");
                        return View("Ok", notifyObj);
                    }

                }

            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }

      

    }
}