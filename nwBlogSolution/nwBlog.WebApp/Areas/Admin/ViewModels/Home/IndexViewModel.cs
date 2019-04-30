using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Home
{
    public class IndexViewModel
    {
        public int UsersCount { get; set; }
        public int AuthorUserCount { get; set; }
        public int BlogsCount { get; set; }
        public int CategoriesCount { get; set; }

        public List<Entities.Blog> BlogsLast5 = new List<Entities.Blog>();

    }
}