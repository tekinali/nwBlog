using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.ViewModels.Home
{
    public class BlogPostViewModel
    {
        public Blog Blog { get; set; }
        public int LikeCount { get; set; }
        public List<Comment> Comments { get; set; }

        public BlogPostViewModel()
        {
            Comments = new List<Comment>();         
        }

    }
}