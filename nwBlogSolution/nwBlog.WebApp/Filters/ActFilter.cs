using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.Entities;
using nwBlog.WebApp.Models;
using nwBlog.BusinessLayer;

namespace nwBlog.WebApp.Filters
{
    public class ActFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string username = "user_logged_out";
            if(CurrentSession.User!=null)
            {
                username = CurrentSession.User.Username;
            }

            Log log = new Log()
            {
                Username = username,
                ActionName = filterContext.ActionDescriptor.ActionName,
                ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                DateTime = DateTime.Now,
                Content = "OnActionExecuted"
            };

            LogManager logManager = new LogManager();

            int db = logManager.Insert(log);
        }

        // action sonrası
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

            string username = "user_logged_out";
            if (CurrentSession.User != null)
            {
                username = CurrentSession.User.Username;
            }


            Log log = new Log()
            {
                Username= username,
                ActionName=filterContext.ActionDescriptor.ActionName,
                ControllerName= filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                DateTime=DateTime.Now,
                Content= "OnActionExecuting"
            };

            LogManager logManager = new LogManager();

            int db = logManager.Insert(log);
            


        }
    }
}