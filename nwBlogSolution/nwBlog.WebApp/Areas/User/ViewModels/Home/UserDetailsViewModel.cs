using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.User.ViewModels.Home
{
    public class UserDetailsViewModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string About { get; set; }

        public List<Blog> Blogs { get; set; }

        public UserDetailsViewModel()
        {
            Blogs = new List<Blog>();
        }
    }
}