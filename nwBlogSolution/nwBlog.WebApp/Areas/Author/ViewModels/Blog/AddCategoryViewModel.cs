using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Author.ViewModels.Blog
{
    public class AddCategoryViewModel
    {
        public Guid BlogId { get; set; }
        public int CategoryId { get; set; }
    }
}