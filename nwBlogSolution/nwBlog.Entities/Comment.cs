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
    [Table("Comments")]
    public class Comment : MyEntityBase<int>
    {
        [DisplayName("Yorum"), Required, StringLength(300)]
        public string Text { get; set; }

        /////////////////////////////////////////////////////////    

        [DisplayName("Kullanıcı")]
        public Guid AppUserId { get; set; }

        [DisplayName("Blog")]
        public Guid BlogId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Blog AppBlog { get; set; }

    }
}
