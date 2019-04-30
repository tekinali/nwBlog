using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Blog
{
    public class EditViewModel
    {
        public Entities.Blog Blog { get; set; }

        public List<Entities.Tag> Tags { get; set; }
        public List<Entities.Category> Categories { get; set; }
        public List<Entities.Comment> Comments { get; set; }
        public List<Entities.Like> Likes { get; set; }
    }
}