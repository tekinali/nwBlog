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
    public class TagManager : ManagerBase<Tag>
    {
        public new BusinessLayerResult<Tag> Insert(Tag data)
        {
            data.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Name).TrimStart().TrimEnd();
            BusinessLayerResult<Tag> res = new BusinessLayerResult<Tag>();

            Tag tag = Find(x => x.BlogId == data.BlogId && x.Name == data.Name);

            if(tag!=null)
            {
                res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Başarısız! Aynı etiket blog içerisinde var!");
                return res;
            }

            int dbResult = base.Insert(new Tag()
            {
                Name=data.Name,
                BlogId=data.BlogId
            });
            if (dbResult > 0)
            {
                // kayıt başarılı
            }
            else
            {
                // kayıt başarısız
                res.AddError(ErrorMessageCode.CategoryCouldNotInserted, "Başarısız! Etiket eklenmedi");
            }

            return res;
        }


    }
}
