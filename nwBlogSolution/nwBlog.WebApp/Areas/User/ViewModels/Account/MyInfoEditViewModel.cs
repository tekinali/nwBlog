using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.User.ViewModels.Account
{
    public class MyInfoEditViewModel
    {
        [DisplayName("Ad"), Required, StringLength(30)]
        public string Name { get; set; }

        [DisplayName("Soyad"), Required, StringLength(30)]
        public string Surname { get; set; }        

        [DisplayName("Hakkımda"), StringLength(500)]
        public string About { get; set; }

        [DisplayName("E-posta"), Required, DataType(DataType.EmailAddress), StringLength(50)]
        public string Email { get; set; }

        [DisplayName("Şehir")]
        public int CityId { get; set; }
    }
}