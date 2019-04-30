using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Blog
{
    public class IndexViewModel
    {
        public List<Entities.Blog> Blogs { get; set; }
        public List<Entities.Category> Categories { get; set; }

        public IndexViewModel()
        {
            Blogs = new List<Entities.Blog>();
            Categories = new List<Entities.Category>();
        }


    }
}