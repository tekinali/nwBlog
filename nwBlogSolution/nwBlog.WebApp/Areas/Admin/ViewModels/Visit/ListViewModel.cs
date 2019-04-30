using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Visit
{
    public class ListViewModel
    {
        public List<LastVisit> LastVisits { get; set; }

        public ListViewModel()
        {
            LastVisits = new List<LastVisit>();
        }
    }
}