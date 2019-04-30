using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.Entities
{
    [Table("UserRoles")]
    public class UserRole : MyEntityBase<Guid>
    {
        public Guid AppUserId { get; set; }
        public Guid AppRoleId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual AppRole AppRole { get; set; }
    }
}
