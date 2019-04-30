using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nwBlog.Entities;
using nwBlog.BusinessLayer;
using nwBlog.WebApp.Areas.Admin.ViewModels.Blog;
using System.Net;
using nwBlog.BusinessLayer.Result;
using nwBlog.WebApp.Helpers;
using nwBlog.WebApp.Filters;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class BlogController : Controller
    {
        private BlogManager _blogManager;
        private CategoryManager _categoryManager;
        private BlogCategoryManager _blogCategoryManager;
        private AppUserManager _userManager;
        private TagManager _tagManager;
        private CommentManager _commentManager;
        private LikeManager _likeManager;

        public BlogController()
        {
            _blogManager = new BlogManager();
            _categoryManager = new CategoryManager();
            _blogCategoryManager = new BlogCategoryManager();
            _userManager = new AppUserManager();
            _tagManager = new TagManager();
            _commentManager = new CommentManager();
            _likeManager = new LikeManager();
        }

        // GET: Admin/Blog
        public ActionResult Index(int? id)
        {
            IndexViewModel model = new IndexViewModel();


            if (id == null || id == -1)
            {
                model.Blogs = _blogManager.List();
            }
            else
            {
                var category = _categoryManager.Find(x => x.Id == id);
                if (category == null)
                {
                    // hatalı kategori seçimi
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                model.Blogs = _blogCategoryManager.ListQueryable().Where(x => x.CategoryId == id).Select(u => u.Blog).ToList();

            }

            foreach (var item in model.Blogs)
            {
                item.Text = HttpUtility.HtmlDecode(item.Text);
            }

            model.Categories = _categoryManager.ListQueryable().OrderBy(x => x.Name).ToList();
            return View(model);
        }

        public ActionResult Search(string q = "")
        {
            q = q.Trim();

            if (q == null || q.Length == 0)
            {
                return RedirectToAction("Index");
            }
            IndexViewModel model = new IndexViewModel();
            model.Blogs= _blogManager.ListQueryable().Where(x => x.Tittle.Contains(q) || x.Text.Contains(q) || x.Summary.Contains(q)).Distinct().ToList();

            model.Categories = _categoryManager.ListQueryable().OrderBy(x => x.Name).ToList();
            return View("Index", model);

        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = _blogManager.Find(x => x.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            DetailsViewModel model = new DetailsViewModel();

            model.Blog = blog;

            model.Tags = blog.Tags;
            model.Comments = blog.Comments;
            model.Likes = blog.Likes;
            model.Categories = blog.BlogCategories.Select(x => x.Category).ToList();

            model.Blog.Text = HttpUtility.HtmlDecode(model.Blog.Text);

            return View(model);
        }

        public ActionResult Create()
        {

            var authorUsers = _userManager.GetAllRoleAuthorUsers();
            var adminUsers = _userManager.GetAllRoleAdmins();
            var catList = _categoryManager.List();

            authorUsers.AddRange(adminUsers);
            authorUsers = authorUsers.OrderBy(x => x.Username).ToList();
                     

            ViewBag.CategoryId = new SelectList(catList, "Id", "Name");
            ViewBag.UserId = new SelectList(authorUsers, "Id", "Username");


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
                    Tittle = model.Tittle,
                    Text = model.Text,
                    Summary = model.Summary,
                    UrlName = model.UrlName,
                    IsDraft = model.IsDraft,
                    AppUserId = model.UserId
                };

                BusinessLayerResult<Blog> res = _blogManager.Insert(blog, model.CategoryId);
                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }
                else
                {
                    // başarılı
                    CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                    return RedirectToAction("Index", "Blog");
                }
            }


            var authorUsers = _userManager.GetAllRoleAuthorUsers();
            var adminUsers = _userManager.GetAllRoleAdmins();
            var catList = _categoryManager.List();

            authorUsers.AddRange(adminUsers);
            authorUsers = authorUsers.OrderBy(x => x.Username).ToList();


            ViewBag.CategoryId = new SelectList(catList, "Id", "Name",model.CategoryId);
            ViewBag.UserId = new SelectList(authorUsers, "Id", "Username",model.UserId);
            return View(model);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Blog blog = _blogManager.Find(x => x.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            return View(blog);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid? id)
        {
            Blog blog = _blogManager.Find(x => x.Id == id);
            BusinessLayerResult<Blog> res = _blogManager.Delete(blog);
            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(blog);
            }
            else
            {
                CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                return RedirectToAction("Index", "Blog");
            }



        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var blog = _blogManager.Find(x => x.Id == id);
            if (blog == null)
            {
                return HttpNotFound();
            }

            EditViewModel model = new EditViewModel();

            model.Blog = blog;
            model.Blog.Text = HttpUtility.HtmlDecode(model.Blog.Text);

            model.Tags = blog.Tags;
            model.Comments = blog.Comments;
            model.Likes = blog.Likes;
            model.Categories = blog.BlogCategories.Select(x => x.Category).ToList();

            var authorUsers = _userManager.GetAllRoleAuthorUsers();
            var adminUsers = _userManager.GetAllRoleAdmins();

            authorUsers.AddRange(adminUsers);
            authorUsers = authorUsers.OrderBy(x => x.Username).ToList();

            ViewBag.UserId = new SelectList(authorUsers, "Id", "Username", model.Blog.AppUserId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(EditViewModel model)
        {
            ModelState.Remove("Tags");
            ModelState.Remove("Categories");
            ModelState.Remove("Comments");
            ModelState.Remove("Likes");
            ModelState.Remove("Blog.ModifiedUsername");
            if (ModelState.IsValid)
            {            
                Blog blog = model.Blog;
                BusinessLayerResult<Blog> res = _blogManager.Update(blog);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    model.Tags = blog.Tags;
                    model.Comments = blog.Comments;
                    model.Likes = blog.Likes;
                    model.Categories = blog.BlogCategories.Select(x => x.Category).ToList();

                    var authorUsers = _userManager.GetAllRoleAuthorUsers();
                    var adminUsers = _userManager.GetAllRoleAdmins();

                    authorUsers.AddRange(adminUsers);
                    authorUsers = authorUsers.OrderBy(x => x.Username).ToList();

                    ViewBag.UserId = new SelectList(authorUsers, "Id", "Username", model.Blog.AppUserId);

                    return View(model);
                }
                else
                {
                    CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                    return RedirectToAction("Details", "Blog", new { @id = blog.Id });
                }

            }
            else
            {
                Blog blog = model.Blog;

                model.Tags = blog.Tags;
                model.Comments = blog.Comments;
                model.Likes = blog.Likes;
                model.Categories = blog.BlogCategories.Select(x => x.Category).ToList();

                var authorUsers = _userManager.GetAllRoleAuthorUsers();
                var adminUsers = _userManager.GetAllRoleAdmins();

                authorUsers.AddRange(adminUsers);
                authorUsers = authorUsers.OrderBy(x => x.Username).ToList();

                ViewBag.UserId = new SelectList(authorUsers, "Id", "Username", model.Blog.AppUserId);


                return View(model);
            }

        }

        public ActionResult DeleteCategory(Guid blogid, int catid)
        {
            BlogCategory bc = _blogCategoryManager.Find(x => x.BlogId == blogid && x.CategoryId == catid);
            if (bc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            _blogCategoryManager.Delete(bc);

            CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
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

            CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
            return RedirectToAction("Edit", "Blog", new { @id = blogid });
        }

        public ActionResult DeleteComment(Guid blogid,int itemId)
        {
            Comment comment = _commentManager.Find(x => x.Id == itemId);
            if(comment==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _commentManager.Delete(comment);
            CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
            return RedirectToAction("Edit", "Blog", new { @id = blogid });
        }

        public ActionResult DeleteLike(Guid blogid, int itemId)
        {
            Like like = _likeManager.Find(x => x.Id == itemId);
            if (like == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _likeManager.Delete(like);

            CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
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

            var catList = _categoryManager.List();

            ViewBag.CategoryId = new SelectList(catList, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(AddCategoryViewModel model)
        {

            if(ModelState.IsValid)
            {
                BlogCategory bc = new BlogCategory()
                {
                    BlogId=model.BlogId,
                    CategoryId=model.CategoryId
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
                    CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                    return RedirectToAction("Edit", "Blog",new { @id=model.BlogId});
                }

            }
            var catList = _categoryManager.List();
            ViewBag.CategoryId = new SelectList(catList, "Id", "Name",model.CategoryId);
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
                    BlogId=model.BlogId,
                    Name= model.TagString
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
                    CacheHelper.RemoveGetBlogsWithOutDraftDeleteFromCache();
                    return RedirectToAction("Edit", "Blog", new { @id = model.BlogId });
                }



            }
            return View(model);
        }




    }
}