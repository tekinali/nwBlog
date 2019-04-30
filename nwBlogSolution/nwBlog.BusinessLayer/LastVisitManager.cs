using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;

namespace nwBlog.BusinessLayer
{
    public class LastVisitManager:ManagerBase<LastVisit>
    {
        UserRoleManager _userRoleManager;
        AppRoleManager _roleManager;

        public LastVisitManager()
        {
            _userRoleManager = new UserRoleManager();
            _roleManager = new AppRoleManager();
        }

        public List<LastVisit> GetAllUsersVisits()
        {
            var roleId = _roleManager.GetUserRoleId();
            var usersId = _userRoleManager.ListQueryable().Where(x => x.AppRoleId == roleId).Select(u=>u.AppUserId).ToList();

            var visistList = ListQueryable().Where(x => usersId.Contains(x.AppUserId)).ToList();

            return visistList;
            
        }

        public List<LastVisit> GetAllAuthorUsersVisits()
        {
            var roleId = _roleManager.GetAuthorUserRoleId();
            var usersId = _userRoleManager.ListQueryable().Where(x => x.AppRoleId == roleId).Select(u => u.AppUserId).ToList();

            var visistsList = ListQueryable().Where(x => usersId.Contains(x.AppUserId)).ToList();

            return visistsList;

        }




    }
}
