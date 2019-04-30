using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using nwBlog.Entities;

namespace nwBlog.WebApp.Areas.Admin.ViewModels.Contact
{
    public class IndexViewModel
    {
        public List<ContactMessage> Mails { get; set; }

        public IndexViewModel()
        {
            Mails = new List<ContactMessage>();
        }
    }
}