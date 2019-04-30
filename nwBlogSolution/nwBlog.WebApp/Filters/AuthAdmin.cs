using nwBlog.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.Entities;
using nwBlog.BusinessLayer;


namespace nwBlog.WebApp.Filters
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {         
            UserRoleManager _userRoleManager = new UserRoleManager();
            AppRoleManager _roleManager = new AppRoleManager();

            var admin = _roleManager.GetAdminRoleId();
            var userRole = _userRoleManager.Find(x => x.AppUserId == CurrentSession.User.Id && x.AppRoleId == admin);

            if (userRole==null)
            {
                filterContext.Result = new RedirectResult("/Error/AccessDenied");
            }


        }
    }
}