using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Home
{
    public class MyLogsViewModel
    {
        public List<Entities.Log> Logs { get; set; }

        public MyLogsViewModel()
        {
            Logs = new List<Entities.Log>();
        }
    }
}