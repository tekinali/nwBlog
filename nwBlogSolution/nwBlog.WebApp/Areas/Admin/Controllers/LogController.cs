using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.BusinessLayer;
using nwBlog.Entities;
using nwBlog.WebApp.Areas.Admin.ViewModels.Log;
using nwBlog.WebApp.Filters;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class LogController : Controller
    {

        private LogManager _logManager;

        public LogController()
        {
            _logManager = new LogManager();
        }


        // GET: Admin/Log
        public ActionResult ListUsers()
        {
            ListViewModel model = new ListViewModel();
            model.Logs = _logManager.GetAllUsersLogs();

            return View(model);
        }

        public ActionResult ListAuthorUsers()
        {
            ListViewModel model = new ListViewModel();
            model.Logs = _logManager.GetAllAuthorUsersLogs();

            return View("ListUsers", model);
        }


    }
}