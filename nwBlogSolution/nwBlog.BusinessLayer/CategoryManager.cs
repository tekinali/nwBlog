using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;
using nwBlog.BusinessLayer.Result;
using System.Globalization;
using nwBlog.Entities.Messages;

namespace nwBlog.BusinessLayer
{
    public class CategoryManager:ManagerBase<Category>
    {        
        BlogCategoryManager _blogCategoryManager;


        public CategoryManager()
        {
            _blogCategoryManager = new BlogCategoryManager();
        }

        public new BusinessLayerResult<Category> Insert(Category data)
        {
            data.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            if (data.Description!=null)
            {
                data.Description = data.Description.TrimStart().TrimEnd();
            }
            

            Category cat = Find(x => x.Name == data.Name);

            BusinessLayerResult<Category> res = new BusinessLayerResult<Category>();         

            if (cat != null) //böyle kategori var
            {
                res.AddError(ErrorMessageCode.CategoryAlreadyExists, "Başarısız! Aynı ada sahip Kategori mevcut!");
            }
            else
            {
                DateTime now = DateTime.Now;
                int dbResult = base.Insert(new Category()
                {
                    Name = data.Name,
                    Description=data.Description      
                });

                if (dbResult > 0)
                {
                    // kayıt başarılı

                }
                else
                {
                    // kayıt başarısız
                    res.AddError(ErrorMessageCode.CategoryCouldNotInserted, "Başarısız! Kategori eklenmedi");
                }

            }
            return res;
        }

        public new BusinessLayerResult<Category> Update(Category data)
        {
            data.Name= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();

            Category db_cat = Find(x => x.Name == data.Name);
            BusinessLayerResult<Category> res = new BusinessLayerResult<Category>();

            res.Result = data;

            if (db_cat != null && db_cat.Id != data.Id)
            {
                res.AddError(ErrorMessageCode.CategoryAlreadyExists, "Kategori adı kayıtlı.");

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Name = data.Name;
            res.Result.Description = data.Description;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.CategoryCouldNotUpdated, "Kategori güncellenemedi.");
            }

            return res;


        }

        public new BusinessLayerResult<Category> Delete(Category data)
        {
            // delete : blogcategory
            BusinessLayerResult<Category> res = new BusinessLayerResult<Category>();
            res.Result = Find(x => x.Id == data.Id);

            var blogcategories = _blogCategoryManager.ListQueryable().Where(x => x.CategoryId == data.Id).ToList();

            // delete blogcategory
            if (blogcategories.Count()>0)
            {
                foreach (var item in blogcategories)
                {
                    if(_blogCategoryManager.Delete(item)==0)
                    {
                        res.AddError(ErrorMessageCode.CategoryCouldNotRemove, "Kategoriye ait blog silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }
            }


            if (base.Delete(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.CategoryCouldNotRemove, "Kategori silinemedi.");
            }

            return res;
        }
    }
}
