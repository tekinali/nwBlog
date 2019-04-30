using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class ListAdminViewModel
    {
        public List<AppUser> Admins { get; set; }

        public ListAdminViewModel()
        {
            Admins = new List<AppUser>();
        }

    }
}