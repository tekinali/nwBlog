using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nwBlog.BusinessLayer;
using nwBlog.BusinessLayer.Result;
using nwBlog.Entities;
using nwBlog.WebApp.Areas.Admin.ViewModels.Category;
using nwBlog.WebApp.Filters;
using nwBlog.WebApp.Helpers;

namespace nwBlog.WebApp.Areas.Admin.Controllers
{
    [Auth]
    [AuthAdmin]
    [ActFilter]
    [Exc]
    public class CategoryController : Controller
    {
        private CategoryManager _categoryManager;

        public CategoryController()
        {
            _categoryManager = new CategoryManager();
        }


        // GET: Admin/Category
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            model.categories = _categoryManager.List();

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            ModelState.Remove("Category.ModifiedOn");
            ModelState.Remove("Category.ModifiedUsername");
            ModelState.Remove("Category.CreatedOn");
            if (ModelState.IsValid)
            {
                Category category = new Category()
                {
                    Name=model.Name,
                    Description=model.Description,
                };
                BusinessLayerResult<Category> res = _categoryManager.Insert(category);

                if (res.Errors.Count > 0)
                {
                    // başarısız
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                }

                else
                {
                    // başarılı          
                    CacheHelper.RemoveGetCategoriesFromCache();
                    return RedirectToAction("Index", "Category");
                }


            }



            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DetailsViewModel model = new DetailsViewModel();
            Category category = _categoryManager.Find(x => x.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }

            model.Category = category;

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EditViewModel model = new EditViewModel();

            Category cat = _categoryManager.Find(x => x.Id == id);

            if (cat == null)
            {
                return HttpNotFound();
            }

            model.Category = cat;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model)
        {
           
            if (ModelState.IsValid)
            {
                Category cat = model.Category;
                BusinessLayerResult<Category> res = _categoryManager.Update(cat);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                else
                {
                    CacheHelper.RemoveGetCategoriesFromCache();
                    return RedirectToAction("Details", "Category", new { @id = cat.Id });
                }
            }
            return View(model);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryManager.Find(x => x.Id == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryManager.Find(x => x.Id == id);
            BusinessLayerResult<Category> res = _categoryManager.Delete(category);

            if (res.Errors.Count > 0)
            {
                res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                return View(category);
            }
            else
            {
                CacheHelper.RemoveGetCategoriesFromCache();
                return RedirectToAction("Index", "Category");
            }
        }





    }
}