using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Author.ViewModels.Blog
{
    public class AddTagViewModel
    {
        public Guid BlogId { get; set; }

        [Required, StringLength(20)]
        public string TagString { get; set; }
    }
}