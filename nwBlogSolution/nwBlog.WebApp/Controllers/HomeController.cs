using nwBlog.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.WebApp.ViewModels.Home;
using PagedList;
using PagedList.Mvc;
using nwBlog.BusinessLayer;
using nwBlog.Entities;
using System.Net;
using System.Globalization;

namespace nwBlog.WebApp.Controllers
{
    public class HomeController : Controller
    {
        BlogCategoryManager _blogCategoryManager;
        CategoryManager _categoryManager;
        BlogManager _blogManager;
        CommentManager _commentManager;
        LikeManager _likeManager;
        ContactManager _contactManager;       

        public HomeController()
        {
            _blogCategoryManager = new BlogCategoryManager();
            _categoryManager = new CategoryManager();
            _blogManager = new BlogManager();
            _commentManager = new CommentManager();
            _likeManager = new LikeManager();
            _contactManager = new ContactManager();                
        }


        // GET: Home
        public ActionResult Index(int Page=1,int? Category=-1)
        {
            if(Category == null || Category == -1)
            {
                ViewBag.CategoryId = null;

                var blogs = CacheHelper.GetBlogsWithOutDraftDeleteFromCache().ToPagedList(Page, 5);
                return View(blogs);
            }
            else
            {
                var category = _categoryManager.Find(x => x.Id == Category);

                if (category==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ViewBag.CategoryId = Category;

                var catBlogs = _blogCategoryManager.ListQueryable().Where(x => x.CategoryId == Category).Select(u => u.Blog).ToList();
                var blogs = catBlogs.Where(x => x.IsDelete == false && x.IsDraft == false).ToList().ToPagedList(Page, 5);
                return View(blogs);
            }     
          
        }               

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactMessage model)
        {
            if(ModelState.IsValid)
            {
                ContactMessage message = new ContactMessage()
                {
                    Name= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Name).Trim(),
                    Surname= CultureInfo.CurrentCulture.TextInfo.ToUpper(model.Surname).Trim(),
                    Email= CultureInfo.CurrentUICulture.TextInfo.ToLower(model.Email).Trim().Replace(" ", string.Empty),
                    Subject=model.Subject.Trim(),
                    Text=model.Text.Trim(),
                    IsRead=false,
                    DateTime=DateTime.Now
                };
                int db_Res = _contactManager.Insert(message);
                if(db_Res>0)
                {
                    ViewBag.Result = true;
                }
                else
                {
                    ViewBag.Result = false;
                }
              
            }
            return View(model);
        }

        public ActionResult BlogPost(Guid? Blog)
        {
            if(Blog==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
      
            BlogPostViewModel model = new BlogPostViewModel();
            model.Blog= CacheHelper.GetBlogsWithOutDraftDeleteFromCache().FirstOrDefault(x => x.Id == Blog);
            model.Comments = _commentManager.ListQueryable().Where(x => x.BlogId == Blog).OrderByDescending(x=>x.CreatedOn).ToList();
            model.LikeCount = _likeManager.ListQueryable().Where(x => x.BlogId == Blog).ToList().Count();
          

            return View(model);
        }

        public ActionResult UserDetails(string User)
        {
            if(User==null || User.Length==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = CacheHelper.GetUsersWithOutDeleteFromCache().Find(x => x.Username == User);
            if(user==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserDetailsViewModel model = new UserDetailsViewModel();

            model.About = user.About;
            model.Name = user.Name;
            model.Surname = user.Surname;
            model.Username = user.Username;
            model.Blogs = user.Blogs;          

            return View(model);
        }

        public ActionResult Search(int Page=1, string q="")
        {
            q = q.Trim();

            if(q==null || q.Length==0)
            {
                return RedirectToAction("Index");
            }
            ViewBag.q = q;

            var bblogs = CacheHelper.GetBlogsWithOutDraftDeleteFromCache().Where(x => x.Tittle.Contains(q) || x.Text.Contains(q) || x.Summary.Contains(q)).Distinct().ToList();
            ViewBag.Count = bblogs.Count();

            var blogs = bblogs.ToPagedList(Page, 5);
          

            return View(blogs);
        }

        [ChildActionOnly]
        public PartialViewResult CategoryList()
        {
            var categoryList = CacheHelper.GetCategoriesFromCache();

            return PartialView("_PartialCategoryList", categoryList);
        }

        [ChildActionOnly]
        public PartialViewResult TagList()
        {
            var tagList = CacheHelper.GetTagsFromCache();

            return PartialView("_PartialTagList", tagList);
        }

        [ChildActionOnly]
        public PartialViewResult PartialSearch()
        {
            return PartialView("_PartialSearch");
        }



    }
}