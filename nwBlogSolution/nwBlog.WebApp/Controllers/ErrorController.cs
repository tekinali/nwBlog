using nwBlog.WebApp.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nwBlog.WebApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult HasError()
        {
            return View();
        }

        public ActionResult AccessDenied()
        {
            ErrorViewModel errorNotifyObj = new ErrorViewModel()
            {
                Title = "Yektkisiz Erişim",
                RedirectingUrl = Url.Action("Login","Account")
            };

            return View("PageError.cshtml", errorNotifyObj);
        }



    }
}