using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nwBlog.BusinessLayer;
using nwBlog.BusinessLayer.Result;
using nwBlog.Entities;
using nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool;
using nwBlog.WebApp.Filters;
using nwBlog.WebApp.Helpers;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class SystemToolController : Controller
    {
        CityManager _cityManager;
        AppRoleManager _roleManager;
        LogManager _logManager;
        LastVisitManager _visitManager;
        BlogManager _blogManager;
        AppUserManager _userManager;
        UserRoleManager _userRoleManager;
        CommentManager _commentManager;
        LikeManager _likeManager;

        public SystemToolController()
        {
            _cityManager = new CityManager();
            _roleManager = new AppRoleManager();
            _logManager = new LogManager();
            _visitManager = new LastVisitManager();
            _blogManager = new BlogManager();
            _userManager = new AppUserManager();
            _userRoleManager = new UserRoleManager();
            _likeManager = new LikeManager();
            _commentManager = new CommentManager();
        }        

        public ActionResult CityList()
        {
            CityListViewModel model = new CityListViewModel();
            model.Cities = _cityManager.List();

            return View(model);
        }

        public ActionResult RoleList()
        {
            RoleListViewModel model = new RoleListViewModel();
            model.Roles = _roleManager.List();

            return View(model);
        }

        public ActionResult LogList()
        {
            LogListViewModel model = new LogListViewModel();
            model.Logs = _logManager.List();

            return View(model);
        }

        public ActionResult VisitList()
        {
            VisitListViewModel model = new VisitListViewModel();
            model.Visits = _visitManager.List();

            return View(model);
        }
       
        public ActionResult RemoveBlogList()
        {
            RemoveBlogListViewModel model = new RemoveBlogListViewModel();

            model.Blogs = _blogManager.List();

            return View(model);
        }

        public ActionResult RemoveUserList()
        {
            RemoveUserListViewModel model = new RemoveUserListViewModel();

            var authorUsers = _userManager.GetAllRoleAuthorUsers();
            var standartUsers = _userManager.GetAllRoleUsers();

            standartUsers.AddRange(authorUsers);

            model.Users = standartUsers;

            return View(model);
        }

        public ActionResult RemoveBlog(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = _blogManager.Find(x => x.Id == id);

            if(blog==null)
            {
                return HttpNotFound();
            }

            return View(blog);
        }

        [HttpPost, ActionName("RemoveBlog")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveBlogConfirmed(Guid id)
        {
            Blog blog = _blogManager.Find(x => x.Id == id);
            BusinessLayerResult<Blog> res = _blogManager.RemoveSystem(blog);
            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(blog);
            }
            else
            {
                CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                return RedirectToAction("RemoveBlogList", "SystemTool");

            }         
        }

        public ActionResult RemoveUser(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = _userManager.Find(x => x.Id == id);

            if(user==null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("RemoveUser")]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUserConfirmed(Guid id)
        {
            AppUser user = _userManager.Find(x => x.Id == id);
            BusinessLayerResult<AppUser> res = _userManager.RemoveSystem(user);

            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(user);
            }
            else
            {               
                CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                return RedirectToAction("RemoveUserList", "SystemTool");

            }
        }

        public ActionResult UserListWithRole(int? id)
        {
            UserListWithRoleViewModel model = new UserListWithRoleViewModel();
            List<AppUser> userList = new List<AppUser>();
         

            if(id==null || id==-1)
            {
                id = 1;
            }

            switch (id)
            {
                case 1:
                    // tümü
                    userList = _userManager.List();
                    break;

                case 2:
                    // kullanıcılar
                    userList = _userManager.GetAllRoleUsers();
                    break;
                case 3:
                    // yazarlar
                    userList = _userManager.GetAllRoleAuthorUsers();
                    break;

                case 4:
                    // yönetciler
                    userList = _userManager.GetAllRoleAdmins();
                    break;

                default:
                    // hatalı kategori seçimi
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);           

            }

            model.Users = userList;               
            

            return View(model);
        }

        public ActionResult EditUser(Guid? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditUserViewModel model = new EditUserViewModel();
            var user = _userManager.Find(x => x.Id == userId);

            model.Name = user.Name;
            model.Surname = user.Surname;
            model.Username = user.Username;
            model.UserId = user.Id;
            model.UserRoles = user.UserRoles;

            List<SelectListItem> RoleListId = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Kullanıcı", Value="1"},
                new SelectListItem() {Text="Yazar", Value="2"},
                new SelectListItem() {Text="Yönetici", Value="3"}
            };

            ViewBag.RoleId = new SelectList(RoleListId,"Value","Text");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if(ModelState.IsValid)
            {
                Guid roleId;
                switch (model.RoleId)
                {
                    case 1:
                        roleId = _roleManager.GetUserRoleId();
                        break;
                    case 2:
                        roleId = _roleManager.GetAuthorUserRoleId();
                        break;
                    case 3:
                        roleId = _roleManager.GetAdminRoleId();
                        break;
                    default:
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                UserRole userRole = new UserRole()
                {
                    AppUserId=model.UserId,
                    AppRoleId=roleId
                };
                BusinessLayerResult<UserRole> res = _userRoleManager.Insert(userRole);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    // başarılı
                    return RedirectToAction("EditUser", "SystemTool",new { @userId =model.UserId});
                }
            }

            var user = _userManager.Find(x => x.Id == model.UserId);
                   
            model.UserRoles = user.UserRoles;

            List<SelectListItem> RoleListId = new List<SelectListItem>()
            {
                new SelectListItem() {Text="Kullanıcı", Value="1"},
                new SelectListItem() {Text="Yazar", Value="2"},
                new SelectListItem() {Text="Yönetici", Value="3"}
            };

            ViewBag.RoleId = new SelectList(RoleListId, "Value", "Text",model.RoleId);
            return View(model);
        }

        public ActionResult DeleteUserRole(Guid usrId, Guid urId)
        {
            UserRole ur = _userRoleManager.Find(x => x.Id == urId);
            if(ur==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _userRoleManager.Delete(ur);

            return RedirectToAction("EditUser", "SystemTool", new { @userId = usrId });
        }


        public ActionResult ListAdmin()
        {
            ListAdminViewModel model = new ListAdminViewModel();
            model.Admins = _userManager.GetAllRoleAdmins();

            return View(model);
        }

        public ActionResult AdminDetails(Guid? userId)
        {
            if (userId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AdminDetailsViewModel model = new AdminDetailsViewModel();
            var usr = _userManager.Find(x => x.Id == userId);          

            if(usr == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var roleId = _roleManager.GetAdminRoleId();
                var userRole = _userRoleManager.Find(x => x.AppUserId == userId && x.AppRoleId== roleId);

                if(userRole ==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                else
                {
                    model.Admin = usr;

                    model.Blogs = _blogManager.ListQueryable().Where(x => x.AppUserId == userId).ToList();
                    model.Comments = _commentManager.ListQueryable().Where(x => x.AppUserId == userId).ToList();
                    model.LastVisits = _visitManager.ListQueryable().Where(x => x.AppUserId == userId).ToList();
                    model.Likes = _likeManager.ListQueryable().Where(x => x.AppUserId == userId).ToList();
                    model.Logs = _logManager.ListQueryable().Where(x => x.Username == usr.Username).ToList();
                    model.UserRoles= _userRoleManager.ListQueryable().Where(x => x.AppUserId == userId).ToList();

                    return View(model);
                }
            }


          
        }



    }
}