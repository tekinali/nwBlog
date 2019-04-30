using nwBlog.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.Entities;
using nwBlog.Entities.ValueObjects;
using nwBlog.BusinessLayer;
using nwBlog.BusinessLayer.Result;
using nwBlog.WebApp.Models.Notification;
using nwBlog.WebApp.Models;
using System.Runtime.Remoting.Contexts;

namespace nwBlog.WebApp.Controllers
{
    public class AccountController : Controller
    {

        AppUserManager _userManager;
        UserRoleManager _userRoleManager;
        AppRoleManager _roleManager;
        LastVisitManager _lastVisitManager;

        public AccountController()
        {
            _userManager = new AppUserManager();
            _userRoleManager = new UserRoleManager();
            _roleManager = new AppRoleManager();
            _lastVisitManager = new LastVisitManager();
        }

        public string GetClientIp()
        {
            var ipAddress = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null && System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"];
            }
            else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                ipAddress = System.Web.HttpContext.Current.Request.UserHostName;
            }

            return ipAddress;
        }

        // GET: Account
        public ActionResult Login()
        {
            Session.Clear();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            // giriş kontrolü ve yönlendirme
            // sessiona kullanıcı bilgi saklama
            if (ModelState.IsValid)
            {
                BusinessLayerResult<AppUser> res = _userManager.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                else
                {
                    // role göre yönlendirme
                    var admin = _roleManager.GetAdminRoleId();
                    var author = _roleManager.GetAuthorUserRoleId();
                    var user = _roleManager.GetUserRoleId();

                    var userRole = _userRoleManager.Find(x => x.AppUserId == res.Result.Id && x.AppRoleId == admin);

                    if (userRole!=null)
                    {
                        LastVisit lv = new LastVisit()
                        {
                            AppUserId=res.Result.Id,
                            DateTime=DateTime.Now,
                            IpAddress= GetClientIp()
                            
                        };

                        int dbres = _lastVisitManager.Insert(lv);

                        CurrentSession.Set<AppUser>("login", res.Result);
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    userRole = _userRoleManager.Find(x => x.AppUserId == res.Result.Id && x.AppRoleId == author);

                    if (userRole != null)
                    {
                        LastVisit lv = new LastVisit()
                        {
                            AppUserId = res.Result.Id,
                            DateTime = DateTime.Now,
                            IpAddress = GetClientIp()

                        };

                        int dbres = _lastVisitManager.Insert(lv);

                        CurrentSession.Set<AppUser>("login", res.Result);
                        return RedirectToAction("Index", "Home", new { area = "Author" });
                    }

                    userRole = _userRoleManager.Find(x => x.AppUserId == res.Result.Id && x.AppRoleId == user);

                    if (userRole != null)
                    {
                        LastVisit lv = new LastVisit()
                        {
                            AppUserId = res.Result.Id,
                            DateTime = DateTime.Now,
                            IpAddress = GetClientIp()

                        };

                        int dbres = _lastVisitManager.Insert(lv);

                        CurrentSession.Set<AppUser>("login", res.Result);
                        return RedirectToAction("Index", "Home", new { area = "User" });
                    }

                    ModelState.AddModelError("", "Giriş yapılamıyor");
                }


            }
            return View(model);
        }

        public ActionResult Register()
        {
            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                BusinessLayerResult<AppUser> res = _userManager.RegisterUser(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title="İşlem Başarılı",
                    RedirectingUrl="/Account/Login"
                };

                notifyObj.Items.Add("Üyeliğinizi tamamlamaya bir adım kaldı.");
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz.");

                ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name", model.CityId);
                return View("Ok", notifyObj);

            }
            ViewBag.CityId = new SelectList(CacheHelper.GetCitiesFromCache(), "Id", "Name");
            return View(model);
        }

        public ActionResult testok()
        {
            OkViewModel notifyObj = new OkViewModel()
            {
                Title = "İşlem Başarılı",
                RedirectingUrl = "/Account/Login"
            };

            notifyObj.Items.Add("Üyeliğinizi tamamlamaya bir adım kaldı.");
            notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz.");
            return View("ok",notifyObj);
        }

        public ActionResult UserActivate(Guid id)
        {
            // kullanıcı aktivasyonu sağlama           


            BusinessLayerResult<AppUser> res = _userManager.ActivateUser(id);

            if (res.Errors.Count > 0)
            {      
                return View("Index","Home");
            }
            OkViewModel okNotifiyObj = new OkViewModel()
            {
                Title = "Hesap Aktfileştirildi.",
                RedirectingUrl = Url.Action("Login","Account")
            };

            okNotifiyObj.Items.Add("Hesabınız aktifleştirildi. Giriş yapabilirsiniz. ");

            return View("Ok", okNotifiyObj);
        }




    }
}