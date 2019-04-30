using nwBlog.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.BusinessLayer;
using nwBlog.Entities;
using nwBlog.WebApp.Helpers;
using nwBlog.WebApp.Areas.Author.ViewModels.Blog;
using nwBlog.BusinessLayer.Result;
using System.Net;
using Microsoft.Security.Application;
using nwBlog.WebApp.Filters;

namespace nwBlog.WebApp.Areas.Author.Controllers
{
    [Auth]
    [AuthAuthor]
    [ActFilter]
    [Exc]
    public class BlogController : Controller
    {
        // GET: Author/Blog

        private AppUserManager _userManager;
        private BlogManager _blogManager;
        private BlogCategoryManager _blogCategoryManager;
        private TagManager _tagManager;
        private CategoryManager _categoryManager;

        public BlogController()
        {
            _userManager = new AppUserManager();
            _blogManager = new BlogManager();
            _blogCategoryManager = new BlogCategoryManager();
            _tagManager = new TagManager();
            _categoryManager = new CategoryManager();
        }

        public ActionResult Index()
        {
            var blogs = _blogManager.ListQueryable().Where(x => x.AppUserId == CurrentSession.User.Id && x.IsDelete==false).ToList();           

            return View(blogs);
        }               

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(CreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                Blog blog = new Blog()
                {
                    Tittle=model.Tittle,
                    Summary=model.Summary,
                    Text=Sanitizer.GetSafeHtmlFragment(model.Text),
                    IsDraft=model.IsDraft,
                    IsDelete=false,
                    AppUserId=CurrentSession.User.Id,
                    UrlName=""
                };

                BusinessLayerResult<Blog> res = _blogManager.Insert(blog, model.CategoryId);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");
                }
                else
                {
                    // başarılı
                    CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                    return RedirectToAction("Index", "Blog");
                }
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");

            return View(model);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = _blogManager.Find(x => x.Id == id && x.AppUserId == CurrentSession.User.Id);

            if (blog == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DetailsViewModel model = new DetailsViewModel();
            model.Blog = blog;
            model.Comments = blog.Comments;
            model.LikeCount = blog.Likes.Count();


            return View(model);
        }
            
        public ActionResult Edit(Guid? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = _blogManager.Find(x => x.Id == id && x.AppUserId == CurrentSession.User.Id);

            if(blog==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditViewModel model = new EditViewModel();

            model.Tittle = blog.Tittle;
            model.Summary = blog.Summary;
            model.IsDraft = blog.IsDraft;
            model.Text = HttpUtility.HtmlDecode(blog.Text);
            model.BlogId = (Guid)id;

            model.Categories = _blogCategoryManager.ListQueryable().Where(x => x.BlogId == id).Select(u => u.Category).ToList();
            model.Tags = _tagManager.ListQueryable().Where(x => x.BlogId == id).ToList();

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(EditViewModel model)
        {
            ModelState.Remove("Tags");
            ModelState.Remove("Categories");
            if (ModelState.IsValid)
            {
                var blog = _blogManager.Find(x => x.Id == model.BlogId);
                blog.IsDraft = model.IsDraft;
                blog.Summary = model.Summary;
                blog.Tittle = model.Tittle;
                blog.Text = Sanitizer.GetSafeHtmlFragment(model.Text);
                blog.UrlName = "";

                BusinessLayerResult<Blog> res = _blogManager.Update(blog);
                if(res.Errors.Count>0)
                {
                    model.Tags = blog.Tags;
                    model.Categories = blog.BlogCategories.Select(x => x.Category).ToList();

                    ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");
                    return View(model);
                }
                else
                {
                    CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                    return RedirectToAction("Index", "Blog");
                }



            }


            return View(model);
        }

        public ActionResult DeleteCategory(Guid blogid, int catid)
        {
            BlogCategory bc = _blogCategoryManager.Find(x => x.BlogId == blogid && x.CategoryId == catid);
            if (bc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _blogCategoryManager.Delete(bc);

            return RedirectToAction("Edit", "Blog", new { @id = blogid });
        }

        public ActionResult DeleteTag(Guid blogid, int tagid)
        {
            Tag tag = _tagManager.Find(x => x.Id == tagid);
            if (tag == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _tagManager.Delete(tag);

            return RedirectToAction("Edit", "Blog", new { @id = blogid });
        }

        public ActionResult AddCategory(Guid? blogid)
        {
            if (blogid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AddCategoryViewModel model = new AddCategoryViewModel();
            model.BlogId = (Guid)blogid;

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(AddCategoryViewModel model)
        {

            if (ModelState.IsValid)
            {
                BlogCategory bc = new BlogCategory()
                {
                    BlogId = model.BlogId,
                    CategoryId = model.CategoryId
                };
                BusinessLayerResult<BlogCategory> res = _blogCategoryManager.Insert(bc);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    // başarılı              
                    return RedirectToAction("Edit", "Blog", new { @id = model.BlogId });
                }

            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Name");
            return View(model);
        }

        public ActionResult AddTag(Guid? blogid)
        {
            if (blogid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AddTagViewModel model = new AddTagViewModel();
            model.BlogId = (Guid)blogid;


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTag(AddTagViewModel model)
        {
            if (ModelState.IsValid)
            {
                Tag tg = new Tag()
                {
                    BlogId = model.BlogId,
                    Name = model.TagString
                };

                BusinessLayerResult<Tag> res = _tagManager.Insert(tg);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    // başarılı              
                    return RedirectToAction("Edit", "Blog", new { @id = model.BlogId });
                }



            }
            return View(model);
        }


    }
}