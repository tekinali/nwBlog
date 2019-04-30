using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Category
{
    public class CreateViewModel
    {
        [DisplayName("Kategori Adı"), Required, StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Açıklama"), StringLength(250)]
        public string Description { get; set; }
    }
}