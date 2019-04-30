using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.User
{
    public class IndexViewModel
    {
        public List<AppUser> Users { get; set; }


        public IndexViewModel()
        {
            Users = new List<AppUser>();
        }


    }
}