using nwBlog.BusinessLayer;
using nwBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace nwBlog.WebApp.Helpers
{
    public class CacheHelper
    {
        public static List<City> GetCitiesFromCache()
        {
            var result = WebCache.Get("cities-cache");

            if (result == null)
            {
                CityManager _cityManager = new CityManager();
                result = _cityManager.ListQueryable().OrderBy(x => x.Name).ToList();

                WebCache.Set("cities-cache", result, 20, true);
            }

            return result;
        }

        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("categories-cache");

            if (result == null)
            {
                CategoryManager _categoryManager = new CategoryManager();
                result = _categoryManager.ListQueryable().OrderBy(x => x.Name).ToList();

                WebCache.Set("categories-cache", result, 20, true);
            }

            return result;
        }

        public static List<Tag> GetTagsFromCache()
        {
            var result = WebCache.Get("tags-cache");

            if (result == null)
            {
                TagManager tagManager = new TagManager();
                result = tagManager.ListQueryable().OrderBy(x => x.CreatedOn).Take(10).ToList();

                WebCache.Set("tags-cache", result, 20, true);
            }

            return result;
        }

        public static List<Blog> GetBlogsWithOutDraftDeleteFromCache()
        {
            var result = WebCache.Get("BlogsWithOutDraftDelete-cache");

            if (result == null)
            {
                BlogManager _blogManager = new BlogManager();
                result = _blogManager.ListWithOutDeleteteDraft();

                WebCache.Set("BlogsWithOutDraftDelete-cache", result, 20, true);
            }

            return result;
        }

        public static List<AppUser> GetUsersWithOutDeleteFromCache()
        {
            var result = WebCache.Get("UsersWithOutDeleteFromCache-cache");

            if (result == null)
            {
                AppUserManager _userManager = new AppUserManager();
                result = _userManager.ListUsersActiveIsOk();

                WebCache.Set("UsersWithOutDeleteFromCache-cache", result, 20, true);
            }

            return result;
        }
        
        //////////////////////////////////////////////////////////////

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }

        public static void RemoveGetCitiesFromCache()
        {
            Remove("cities-cache");
        }

        public static void RemoveGetCategoriesFromCache()
        {
            Remove("categories-cache");
        }

        public static void RemoveGetTagsFromCache()
        {
            Remove("tags-cache");
        }

        public static void RemoveGetBlogsWithOutDraftDeleteFromCache()
        {
            Remove("BlogsWithOutDraftDelete-cache");
        }


        public static void RemoveUsersWithOutDeleteFromCache()
        {
            Remove("UsersWithOutDeleteFromCache-cache");
        }

    }
}