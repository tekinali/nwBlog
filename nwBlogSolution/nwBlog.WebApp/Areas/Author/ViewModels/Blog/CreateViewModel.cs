using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nwBlog.WebApp.Areas.Author.ViewModels.Blog
{
    public class CreateViewModel
    {
        [DisplayName("Başlık"), Required, StringLength(250)]
        public string Tittle { get; set; }

        [DisplayName("Özet"), Required, StringLength(1000)]
        public string Summary { get; set; }

        [DisplayName("İçerik"), Required, StringLength(5000)]
        [AllowHtml]
        public string Text { get; set; }

        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }        

        [DisplayName("Kategori"), Required]
        public int CategoryId { get; set; }
    }
}