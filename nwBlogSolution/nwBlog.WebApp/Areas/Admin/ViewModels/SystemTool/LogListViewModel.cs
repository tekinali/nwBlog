using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class LogListViewModel
    {
        public List<Entities.Log> Logs { get; set; }

        public LogListViewModel()
        {
            Logs = new List<Entities.Log>();
        }
    }
}