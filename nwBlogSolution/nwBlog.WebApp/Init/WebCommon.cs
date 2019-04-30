using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;
using nwBlog.Common;
using nwBlog.WebApp.Models;

namespace nwBlog.WebApp.Init
{
    public class WebCommon:ICommon
    {
        public string GetCurrentUsername()
        {
            AppUser user = CurrentSession.User;

            if (user != null)
                return user.Username;
            else
                return "system";
        }
    }
}