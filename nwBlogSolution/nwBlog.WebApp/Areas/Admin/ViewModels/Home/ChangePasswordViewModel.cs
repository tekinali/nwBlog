using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Home
{
    public class ChangePasswordViewModel
    {
        [DisplayName("Eski Şifreniz"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password),
            StringLength(16, MinimumLength = 6, ErrorMessage = "{0} min. {2} - max. {1} karakter olmalı.")]
        public string Password { get; set; }

        [DisplayName("Eski Şifreniz (Tekrar)"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password),
            StringLength(16, MinimumLength = 6, ErrorMessage = "{0} min. {2} - max. {1} karakter olmalı."), Compare(nameof(Password), ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        public string RePassword { get; set; }

        [DisplayName("Yeni Şifreniz"), Required(ErrorMessage = "{0} alanı boş geçilemez"), DataType(DataType.Password),
           StringLength(16, MinimumLength = 6, ErrorMessage = "{0} min. {2} - max. {1} karakter olmalı.")]
        public string NewPassword { get; set; }
    }
}