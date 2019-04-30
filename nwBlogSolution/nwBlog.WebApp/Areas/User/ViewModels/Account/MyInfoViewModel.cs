using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.User.ViewModels.Account
{
    public class MyInfoViewModel
    {
        public AppUser User { get; set; }
    }
}