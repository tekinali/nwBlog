using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class AdminDetailsViewModel
    {
        public AppUser Admin { get; set; }

        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
        public List<LastVisit> LastVisits { get; set; }
        public List<Entities.Log> Logs { get; set; }
        public List<UserRole> UserRoles { get; set; }
        public List<Entities.Blog> Blogs { get; set; }

        public AdminDetailsViewModel()
        {
            Likes = new List<Like>();
            Comments = new List<Comment>();
            LastVisits = new List<LastVisit>();
            Logs = new List<Entities.Log>();
            UserRoles = new List<UserRole>();
            Blogs = new List<Entities.Blog>();
        }


    }
}