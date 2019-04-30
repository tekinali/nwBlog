using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Home
{
    public class MyCommnentsViewModel
    {
        public List<Comment> Comments { get; set; }

        public MyCommnentsViewModel()
        {
            Comments = new List<Comment>();
        }
    }
}