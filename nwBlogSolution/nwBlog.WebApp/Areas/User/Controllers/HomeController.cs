using nwBlog.BusinessLayer;
using nwBlog.WebApp.Helpers;
using nwBlog.WebApp.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nwBlog.Entities;
using nwBlog.WebApp.Areas.User.ViewModels.Home;
using nwBlog.BusinessLayer.Result;
using nwBlog.WebApp.Filters;

namespace nwBlog.WebApp.Areas.User.Controllers
{
    [Auth]
    [AuthUser]
    [ActFilter]
    [Exc]
    public class HomeController : Controller
    {
        BlogCategoryManager _blogCategoryManager;
        CategoryManager _categoryManager;
        BlogManager _blogManager;
        CommentManager _commentManager;
        LikeManager _likeManager;
        ContactManager _contactManager;
        AppUserManager _userManager;

        public HomeController()
        {
            _blogCategoryManager = new BlogCategoryManager();
            _categoryManager = new CategoryManager();
            _blogManager = new BlogManager();
            _commentManager = new CommentManager();
            _likeManager = new LikeManager();
            _contactManager = new ContactManager();
            _userManager = new AppUserManager();

        }

        // GET: User/Home
        public ActionResult Index(int Page = 1, int? Category = -1)
        {       
            if (Category == null || Category == -1)
            {
                ViewBag.CategoryId = null;

                var blogs = CacheHelper.GetBlogsWithOutDraftDeleteFromCache().ToPagedList(Page, 5);
                return View(blogs);
            }
            else
            {
                var category = _categoryManager.Find(x => x.Id == Category);

                if (category == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ViewBag.CategoryId = Category;

                var catBlogs = _blogCategoryManager.ListQueryable().Where(x => x.CategoryId == Category).Select(u => u.Blog).ToList();
                var blogs = catBlogs.Where(x => x.IsDelete == false && x.IsDraft == false).ToList().ToPagedList(Page, 5);
                return View(blogs);
            }

        }

        public ActionResult Search(int Page = 1, string q = "")
        {
            q = q.Trim();

            if (q == null || q.Length == 0)
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

        public ActionResult BlogPost(Guid? Blog)
        {
           
            if (Blog == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPostViewModel model = new BlogPostViewModel();
            model.Blog = CacheHelper.GetBlogsWithOutDraftDeleteFromCache().FirstOrDefault(x => x.Id == Blog);
            model.Comments = _commentManager.ListQueryable().Where(x => x.BlogId == Blog).OrderByDescending(x => x.CreatedOn).ToList();
            model.LikeCount = _likeManager.ListQueryable().Where(x => x.BlogId == Blog).ToList().Count();
            model.IsLiked = _likeManager.Find(x => x.BlogId == Blog && x.AppUserId == CurrentSession.User.Id)==null ? false : true;


            return View(model);
        }

        public ActionResult SetLikeState(Guid? Blog)
        {
            if (Blog == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Like like = new Like()
            {
                BlogId=(Guid)Blog,
                AppUserId= CurrentSession.User.Id
            };

            BusinessLayerResult<Like> res = _likeManager.SetLikeState(like);

            return RedirectToAction("BlogPost", "Home", new { @Blog = like.BlogId });
        }

        public ActionResult UserDetails(string User)
        {
            if (User == null || User.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = CacheHelper.GetUsersWithOutDeleteFromCache().Find(x => x.Username == User);
            if (user == null)
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


    }
}