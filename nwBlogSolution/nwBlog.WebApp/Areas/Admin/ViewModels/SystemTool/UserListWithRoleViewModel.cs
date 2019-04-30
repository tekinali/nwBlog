using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class UserListWithRoleViewModel
    {
        public List<AppUser> Users { get; set; }

        public UserListWithRoleViewModel()
        {
            Users = new List<AppUser>();
        }


    }
}