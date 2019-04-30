using nwBlog.BusinessLayer;
using nwBlog.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nwBlog.WebApp.Filters
{
    public class AuthUser : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            UserRoleManager _userRoleManager = new UserRoleManager();
            AppRoleManager _roleManager = new AppRoleManager();

            var role = _roleManager.GetUserRoleId();
            var userRole = _userRoleManager.Find(x => x.AppUserId == CurrentSession.User.Id && x.AppRoleId == role);

            if (userRole == null)
            {
                filterContext.Result = new RedirectResult("/Error/AccessDenied");
            }
        }
    }
}