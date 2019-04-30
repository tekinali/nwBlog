using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class CityListViewModel
    {
        public List<Entities.City> Cities { get; set; }

        public CityListViewModel()
        {
            Cities = new List<Entities.City>();
        }


    }
}