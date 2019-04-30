using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Blog
{
    public class AddCategoryViewModel
    {
        public Guid BlogId { get; set; }
        public int CategoryId { get; set; }


    }
}