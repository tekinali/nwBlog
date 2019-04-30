using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class RemoveBlogListViewModel
    {
        public List<Entities.Blog> Blogs { get; set; }

        public RemoveBlogListViewModel()
        {
            Blogs = new List<Entities.Blog>();
        }
    }
}