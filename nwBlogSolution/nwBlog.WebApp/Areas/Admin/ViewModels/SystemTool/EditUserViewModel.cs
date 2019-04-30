using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class EditUserViewModel
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public Guid UserId { get; set; }

        [DisplayName("Rol"),Required]
        public int RoleId { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public EditUserViewModel()
        {
            UserRoles = new List<UserRole>();
        }

    }
}