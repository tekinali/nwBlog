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
    public class UserRoleManager:ManagerBase<UserRole>
    {
        AppRoleManager _roleManager;

        public UserRoleManager()
        {
            _roleManager = new AppRoleManager();
        }

        public int GetUsersCount()
        {
            var Id = _roleManager.GetUserRoleId();
            int count = ListQueryable().Where(x => x.AppRoleId == Id).ToList().Count();
            return count;
        }

        public int GetAuthorUserCount()
        {
            var Id = _roleManager.GetAuthorUserRoleId();
            int count = ListQueryable().Where(x => x.AppRoleId == Id).ToList().Count();
            return count;
        }

        public int GetAdmisCount()
        {
            var Id = _roleManager.GetAdminRoleId();
            int count = ListQueryable().Where(x => x.AppRoleId == Id).ToList().Count();
            return count;
        }

        public new BusinessLayerResult<UserRole> Insert(UserRole data)
        {
            UserRole userRole = Find(x => x.AppRoleId == data.AppRoleId && x.AppUserId == data.AppUserId);
            BusinessLayerResult<UserRole> res = new BusinessLayerResult<UserRole>();

            if(userRole != null)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Başarısız! Kullanıcı aynı role sahip.");
                return res;
            }

            int dbResult = base.Insert(new UserRole()
            {
                AppRoleId=data.AppRoleId,
                AppUserId=data.AppUserId
            });
            if (dbResult > 0)
            {
                // kayıt başarılı

            }
            else
            {
                // kayıt başarısız
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Başarısız! Rol eklenmedi");
            }


            return res;
        }




    }
}
