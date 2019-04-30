using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Category
{
    public class IndexViewModel
    {
        public List<Entities.Category> categories { get; set; }

        public IndexViewModel()
        {
            categories = new List<Entities.Category>();
        }

    }
}