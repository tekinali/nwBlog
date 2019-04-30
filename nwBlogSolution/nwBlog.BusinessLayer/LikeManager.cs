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
    public class LikeManager : ManagerBase<Like>
    {
        public BusinessLayerResult<Like> SetLikeState(Like data)
        {
            var like = Find(x => x.BlogId == data.BlogId && x.AppUserId == data.AppUserId);
            BusinessLayerResult<Like> res = new BusinessLayerResult<Like>();
            if(like!=null)
            {
                //like kaldır

                res.Result = like;
                if (base.Delete(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Beğeni silinemedi.");
                }

            }
            else
            {
                // beğen
                int dbResult = base.Insert(new Like()
                {
                    AppUserId=data.AppUserId,
                    BlogId=data.BlogId
                });
                if(dbResult==0)
                {
                    res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Beğeni eklenmedi.");
                }
            }
            return res;
        }


    }
}
