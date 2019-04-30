using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Author.ViewModels.Blog
{
    public class DetailsViewModel
    {
        public Entities.Blog Blog { get; set; }
        public int LikeCount { get; set; }
        public List<Comment> Comments { get; set; }

        public DetailsViewModel()
        {
            Comments = new List<Comment>();
        }
    }
}