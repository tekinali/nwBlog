using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nwBlog.Entities;
using nwBlog.BusinessLayer.Abstract;

namespace nwBlog.BusinessLayer
{
    public class AppRoleManager:ManagerBase<AppRole>
    {
        public Guid GetUserRoleId()
        {
            var id = Find(x => x.Name == "User").Id;
            return id;
        }

        public Guid GetAuthorUserRoleId()
        {
            var id = Find(x => x.Name == "AuthorUser").Id;
            return id;
        }

        public Guid GetAdminRoleId()
        {
            var id = Find(x => x.Name == "Admin").Id;
            return id;
        }


    }
}
