using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.Entities
{
    [Table("AppRoles")]
    public class AppRole : MyEntityBase<Guid>
    {
        [DisplayName("Rol Adı"), Required, StringLength(20)]
        public string Name { get; set; }

        [DisplayName("Açıklama"), StringLength(150)]
        public string Description { get; set; }

        ///////////////////////////////////////////////////
        
        public virtual List<UserRole> UserRoles { get; set; }

        public AppRole()
        {
            UserRoles = new List<UserRole>();
        }

    }
}
