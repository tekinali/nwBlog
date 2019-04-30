using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;
using nwBlog.BusinessLayer.Result;
using nwBlog.Entities.Messages;

namespace nwBlog.BusinessLayer
{
    public class BlogCategoryManager:ManagerBase<BlogCategory>
    {
        public new BusinessLayerResult<BlogCategory> Insert(BlogCategory data)
        {
            BlogCategory blogCategory = Find(x => x.BlogId == data.BlogId && x.CategoryId == data.CategoryId);

            BusinessLayerResult<BlogCategory> res = new BusinessLayerResult<BlogCategory>();

            if(blogCategory!=null) // böyle bir kayıt var
            {
                res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Başarısız! Aynı kategori blog içerisinde var!");
                return res;
            }

            int dbResult = base.Insert(new BlogCategory()
            {
                CategoryId=data.CategoryId,
                BlogId=data.BlogId
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

            return res;
        }




    }
}
