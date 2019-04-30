using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.Entities
{
    [Table("BlogCategories")]
    public class BlogCategory : MyEntityBase<int>
    {
        [DisplayName("Blog")]
        public Guid BlogId { get; set; }

        [DisplayName("Kategori")]
        public int CategoryId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Category Category { get; set; }
    }
}
