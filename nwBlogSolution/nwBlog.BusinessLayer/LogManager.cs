using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;

namespace nwBlog.BusinessLayer
{
    public class LogManager:ManagerBase<Log>
    {
        UserRoleManager _userRoleManager;
        AppRoleManager _roleManager;

        public LogManager()
        {
            _userRoleManager = new UserRoleManager();
            _roleManager = new AppRoleManager();
        }

        public List<Log> GetAllUsersLogs()
        {
            var roleId = _roleManager.GetUserRoleId();
            var userName = _userRoleManager.ListQueryable().Where(x => x.AppRoleId == roleId).Select(u => u.AppUser.Username).ToList();

            var logList = ListQueryable().Where(x => userName.Contains(x.Username)).ToList();


            return logList;
        }

        public List<Log> GetAllAuthorUsersLogs()
        {
            var roleId = _roleManager.GetAuthorUserRoleId();
            var userName = _userRoleManager.ListQueryable().Where(x => x.AppRoleId == roleId).Select(u => u.AppUser.Username).ToList();

            var logList = ListQueryable().Where(x => userName.Contains(x.Username)).ToList();


            return logList;
        }


    }
}
