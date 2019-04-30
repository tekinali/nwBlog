using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Home
{
    public class MyBlogsViewModel
    {
        public List<Entities.Blog> Blogs { get; set; }
    }
}