using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nwBlog.BusinessLayer;
using nwBlog.BusinessLayer.Result;
using nwBlog.Entities;
using nwBlog.WebApp.Areas.Admin.ViewModels.User;
using nwBlog.WebApp.Filters;
using nwBlog.WebApp.Helpers;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class UserController : Controller
    {
        private AppUserManager _userManager;
        private UserRoleManager _userRoleManager;
        private LogManager _logManager;

        public UserController()
        {
            _userManager = new AppUserManager();
            _userRoleManager = new UserRoleManager();
            _logManager = new LogManager();
        }


        // GET: Admin/User
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            model.Users = _userManager.GetAllRoleUsers();


            return View(model);
        }

        public ActionResult Create()
        {

            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            ModelState.Remove("User.CreatedOn");
            ModelState.Remove("User.ModifiedOn");
            ModelState.Remove("User.ModifiedUsername");
            ModelState.Remove("User.ProfileImageFilename");            
            
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    Username=model.User.Username,
                    Name=model.User.Name,
                    Surname=model.User.Surname,
                    Email = model.User.Email,
                    Password = model.User.Password,
                    About = model.User.About,
                    IsActive=model.User.IsActive,
                    CityId = model.User.CityId
                    
                };
                BusinessLayerResult<AppUser> res = _userManager.InsertFromAdmin(user, AppUserManager.UserType.User);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    // başarılı
                    return RedirectToAction("Index", "User");
                }

            }


            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.User.CityId);
            return View();
        }



        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DetailsViewModel model = new DetailsViewModel();

            AppUser user = _userManager.Find(x => x.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            model.User = user;

            model.Comments = user.Comments;
            model.Likes = user.Likes;
            model.LastVisits = user.LastVisits;
            model.Roles = user.UserRoles.Select(x => x.AppRole).ToList();
            model.Logs = _logManager.ListQueryable().Where(x => x.Username == user.Username).ToList();


            return View(model);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppUser user = _userManager.Find(x => x.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            EditViewModel model = new EditViewModel();
            model.User = user;

            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.User.CityId);


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
            ModelState.Remove("User.ModifiedOn");
            ModelState.Remove("User.ModifiedUsername");
            ModelState.Remove("User.Password");
            ModelState.Remove("User.ProfileImageFilename");
            ModelState.Remove("User.ActivateGuid");
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
                    return RedirectToAction("Details", new { id = model.User.Id });
                }

            }


            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.User.CityId);
            return View(model);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AppUser user = _userManager.Find(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            AppUser user = _userManager.Find(x => x.Id == id);
            BusinessLayerResult<AppUser> res = _userManager.Delete(user);

            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(user);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }

            
        }

    }
}