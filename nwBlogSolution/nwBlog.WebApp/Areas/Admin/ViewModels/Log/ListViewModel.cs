using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Log
{
    public class ListViewModel
    {
        public List<Entities.Log> Logs { get; set; }

        public ListViewModel()
        {
            Logs = new List<Entities.Log>();
        }


    }
}