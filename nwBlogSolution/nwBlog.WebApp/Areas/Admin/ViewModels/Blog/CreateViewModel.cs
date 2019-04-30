using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Blog
{
    public class CreateViewModel
    {

        [DisplayName("Başlık"), Required, StringLength(250)]
        public string Tittle { get; set; }

        [DisplayName("Özet"), Required, StringLength(1000)]
        public string Summary { get; set; }

        [DisplayName("İçerik"), Required, StringLength(5000)]
        public string Text { get; set; }

        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }     

        [ScaffoldColumn(false), StringLength(20)]
        public string UrlName { get; set; }

        [DisplayName("Kullanıcı"),Required]
        public Guid UserId { get; set; }

        [DisplayName("Kategori"),Required]
        public int CategoryId { get; set; }
    }
}