using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.Entities;
using nwBlog.WebApp.Areas.Admin.ViewModels.Visit;
using nwBlog.BusinessLayer;
using nwBlog.WebApp.Filters;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class VisitController : Controller
    {
        private LastVisitManager _lastVisitManager;

        public VisitController()
        {
            _lastVisitManager = new LastVisitManager();
        }

        // GET: Admin/Visit
        public ActionResult ListUsers()
        {
            ListViewModel model = new ListViewModel();
            model.LastVisits = _lastVisitManager.GetAllUsersVisits();

            return View(model);
        }

        public ActionResult ListAuthorUsers()
        {
            ListViewModel model = new ListViewModel();
            model.LastVisits = _lastVisitManager.GetAllAuthorUsersVisits();

            return View("ListUsers",model);
        }


    }
}