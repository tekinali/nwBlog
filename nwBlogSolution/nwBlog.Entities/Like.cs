using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.Entities
{
    [Table("Likes")]
    public class Like : MyEntityBase<int>
    {
        [DisplayName("Kullanıcı")]
        public Guid AppUserId { get; set; }

        [DisplayName("Blog")]
        public Guid BlogId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Blog Blog { get; set; }

    }
}
