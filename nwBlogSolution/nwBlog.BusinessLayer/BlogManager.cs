using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;
using nwBlog.BusinessLayer.Result;
using nwBlog.Entities.Messages;
using System.Globalization;
using System.Web;

namespace nwBlog.BusinessLayer
{
    public class BlogManager : ManagerBase<Blog>
    {
        private LikeManager _likeManager;
        private CommentManager _commentManager;
        private TagManager _tagManager;
        private BlogCategoryManager _blogCategoryManager;
        private BlogImageManager _blogImageManager;


        public BlogManager()
        {
            _likeManager = new LikeManager();
            _commentManager = new CommentManager();
            _tagManager = new TagManager();
            _blogCategoryManager = new BlogCategoryManager();
            _blogImageManager = new BlogImageManager();
        }

        public new BusinessLayerResult<Blog> Delete(Blog data)
        {
            BusinessLayerResult<Blog> res = new BusinessLayerResult<Blog>();
            res.Result = data;

            res.Result.IsDelete = true;
            res.Result.IsDraft = true;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Blog silinemedi");
            }

            return res;
        }


        public new BusinessLayerResult<Blog> Update(Blog data)
        {
            data.Tittle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Tittle).TrimStart().TrimEnd();
            if (!string.IsNullOrEmpty(data.UrlName))
            {
                data.UrlName = CultureInfo.CurrentCulture.TextInfo.ToLower(data.UrlName).Trim().Replace(" ", string.Empty);
            }


            BusinessLayerResult<Blog> res = new BusinessLayerResult<Blog>();
            Blog db_blog = new Blog();

            if (!string.IsNullOrEmpty(data.UrlName))
            {
                db_blog = Find(x => x.UrlName == data.UrlName || x.Tittle == data.Tittle);

                if (db_blog != null && db_blog.Id != data.Id)
                {
                    if (db_blog.UrlName == data.UrlName && data.UrlName != null)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Aynı Url Adına sahipt blog var.");
                    }

                    if (db_blog.Tittle == data.Tittle)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Aynı Başlığa sahip blog var.");
                    }
                    return res;
                }             
             
            }
            else
            {
                db_blog = Find(x => x.Tittle == data.Tittle);

                if (db_blog != null && db_blog.Id != data.Id)
                {
                    res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Aynı Başlığa sahip blog var.");
                    return res;

                }

            }


            res.Result = Find(x => x.Id == data.Id);

            res.Result.IsDelete = data.IsDelete;
            res.Result.IsDraft = data.IsDelete == true ? true : data.IsDraft;
            res.Result.Summary = data.Summary;
            res.Result.Text = HttpUtility.HtmlEncode(data.Text);
            res.Result.Tittle = data.Tittle;
            if (data.UrlName != null)
            {
                res.Result.UrlName = data.UrlName;
            }
            res.Result.AppUserId = data.AppUserId;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Blog güncellenemedi.");
            }


            return res;
        }

        public BusinessLayerResult<Blog> Insert(Blog data, int categoryId)
        {
            data.Tittle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(data.Tittle).TrimStart().TrimEnd();
            if (data.UrlName != null)
            {
                data.UrlName = CultureInfo.CurrentCulture.TextInfo.ToLower(data.UrlName).Trim().Replace(" ", string.Empty);
            }

            BusinessLayerResult<Blog> res = new BusinessLayerResult<Blog>();
            Blog db_blog = new Blog();

            if (data.UrlName != null)
            {
                db_blog = Find(x => x.UrlName == data.UrlName || x.Tittle == data.Tittle);

                if (db_blog != null)
                {
                    if (db_blog.UrlName == data.UrlName && data.UrlName != null)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Aynı Url Adına sahipt blog var.");
                    }

                    if (db_blog.Tittle == data.Tittle)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Aynı Başlığa sahip blog var.");
                    }
                    return res;
                }
              
            }
            else
            {
                db_blog = Find(x => x.Tittle == data.Tittle);

                if (db_blog != null)
                {
                    res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Aynı Başlığa sahip blog var.");
                    return res;

                }
            }

            int dbResult = base.Insert(new Blog()
            {
                Tittle = data.Tittle,
                Text = HttpUtility.HtmlEncode(data.Text),
                Summary = data.Summary.Trim(),
                IsDelete = false,
                IsDraft = data.IsDraft,
                UrlName = data.UrlName != null ? data.UrlName : "",
                AppUserId = data.AppUserId
            });

            // insert başarılı
            if (dbResult > 0)
            {
                res.Result = Find(x => x.Tittle == data.Tittle);

                BlogCategory bc = new BlogCategory()
                {
                    BlogId = res.Result.Id,
                    CategoryId = categoryId
                };

                BusinessLayerResult<BlogCategory> res2 = _blogCategoryManager.Insert(bc);
                if (res2.Errors.Count() > 0)
                {

                    //res2.Errors.ForEach(x => res.AddError(x.Code,x.Message));

                    res.AddError(ErrorMessageCode.BlogCouldNotUpdated, "Bloğa ait kategori eklenirken hata oluştu.İşlem tamamlanamadı.");
                    return res;
                }


            }




            return res;
        }

        public BusinessLayerResult<Blog> RemoveSystem(Blog data)
        {

            // delete : like, comment, blogcategory, blogimage, tag
            BusinessLayerResult<Blog> res = new BusinessLayerResult<Blog>();

            res.Result = Find(x => x.Id == data.Id);

            var blikes = _likeManager.ListQueryable().Where(x => x.BlogId == data.Id).ToList();
            var bcomments = _commentManager.ListQueryable().Where(x => x.BlogId == data.Id).ToList();
            var bcategory = _blogCategoryManager.ListQueryable().Where(x => x.BlogId == data.Id).ToList();
            var bimage = _blogImageManager.ListQueryable().Where(x => x.BlogId == data.Id).ToList();
            var btag = _tagManager.ListQueryable().Where(x => x.BlogId == data.Id).ToList();


            // delete likes
            if (blikes.Count() > 0)
            {
                foreach (var item in blikes)
                {
                    if (_likeManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Bloğa ait beğeni silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }

            }

            // delete comment
            if (bcomments.Count() > 0)
            {
                foreach (var item in bcomments)
                {
                    if (_commentManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Bloğa ait yorum silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }

            }

            // delete blogcategories
            if (bcategory.Count() > 0)
            {
                foreach (var item in bcategory)
                {
                    if (_blogCategoryManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Bloğa ait kategori silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }

            }

            // delete blogimages
            if (bimage.Count() > 0)
            {
                foreach (var item in bimage)
                {
                    if (_blogImageManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Bloğa ait resim silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }

            }

            // delete tags
            if (btag.Count() > 0)
            {
                foreach (var item in btag)
                {
                    if (_tagManager.Delete(item) == 0)
                    {
                        res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Bloğa ait etiket silinirken hata oluştu. İşlem tamamlanamadı.");
                        return res;
                    }
                }

            }

            if (base.Delete(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.BlogCouldNotRemove, "Blog silinemedi.");
            }


            return res;
        }

        public List<Blog> ListWithOutDeleteteDraft()
        {
            var blogList = ListQueryable().Where(x => x.IsDraft == false && x.IsDelete == false).ToList();

            foreach (var item in blogList)
            {
                item.Text = HttpUtility.HtmlDecode(item.Text);
            }

            return blogList;
        }

    }
}
