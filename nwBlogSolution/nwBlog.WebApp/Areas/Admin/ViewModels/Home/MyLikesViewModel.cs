using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Home
{
    public class MyLikesViewModel
    {
        public List<Like> Likes { get; set; }

        public MyLikesViewModel()
        {
            Likes = new List<Like>();
        }
    }
}