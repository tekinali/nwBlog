using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.User.ViewModels.Account
{
    public class CreateCommentViewModel
    {
        [DisplayName("Yorum"), Required, StringLength(300)]
        public string Text { get; set; }

        public Guid BlogId { get; set; }
    }
}