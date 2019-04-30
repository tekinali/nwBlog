using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.WebApp.Areas.Admin.ViewModels.Contact;
using nwBlog.BusinessLayer;
using System.Net;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Result;
using nwBlog.WebApp.Models;
using nwBlog.WebApp.Filters;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class ContactController : Controller
    {

        ContactManager _contactManager;
        SentMailManager _sentMailManager;

        public ContactController()
        {
            _contactManager = new ContactManager();
            _sentMailManager = new SentMailManager();
        }


        // GET: Admin/Contact
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.Mails = _contactManager.List();

            return View(model);
        }

        [ChildActionOnly]
        public PartialViewResult GetMessagePartial()
        {
            return PartialView("_MessagePartialMenu");
        }

        public ActionResult Details(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactMessage model = _contactManager.Find(x => x.Id==id);

            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _contactManager.Read((int)id);

            return View(model);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SentMail model)
        {
            ModelState.Remove("DateTime");
            ModelState.Remove("Username");
            if (ModelState.IsValid)
            {
                SentMail sm = new SentMail()
                {
                    Subject=model.Subject,
                    Text=model.Text,
                    ToEmail=model.ToEmail,
                    Username= CurrentSession.User.Username,
                    DateTime=DateTime.Now                    
                };

                BusinessLayerResult<SentMail> res = _sentMailManager.Insert(sm);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    // başarılı                  
                    return RedirectToAction("Index", "Contact");
                }



            }
            return View(model);
        }

        public ActionResult SentMails()
        {
            var mails = _sentMailManager.List();
            return View(mails);
        }

        public ActionResult SentMailDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SentMail model = _sentMailManager.Find(x => x.Id == id);

            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(model);
        }

        public ActionResult DeleteSentMail(int? id)
        {
            var mail = _sentMailManager.Find(x => x.Id==id);
            int res = _sentMailManager.Delete(mail);

            return RedirectToAction("SentMails", "Contact");
        }

        public ActionResult DeleteContactMessage(int? id)
        {
            var message = _contactManager.Find(x => x.Id == id);
            int res = _contactManager.Delete(message);

            return RedirectToAction("Index", "Contact");
        }

    }
}