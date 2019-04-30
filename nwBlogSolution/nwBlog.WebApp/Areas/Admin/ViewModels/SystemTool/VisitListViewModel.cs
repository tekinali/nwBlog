using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.SystemTool
{
    public class VisitListViewModel
    {
        public List<Entities.LastVisit> Visits { get; set; }

        public VisitListViewModel()
        {
            Visits = new List<Entities.LastVisit>();
        }


    }
}