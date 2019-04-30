using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class RoleListViewModel
    {
        public List<Entities.AppRole> Roles { get; set; }

        //public int UserCount { get; set; }
        //public int AuthorUserCount { get; set; }
        //public int AdminCount { get; set; }

        public RoleListViewModel()
        {
            Roles = new List<Entities.AppRole>();
        }

    }
}