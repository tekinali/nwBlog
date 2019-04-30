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
    [Table("BlogImages")]
    public class BlogImage : MyEntityBase<int>
    {
        [StringLength(50), Required, ScaffoldColumn(false)]
        public string Filename { get; set; }

        [StringLength(10), Required]
        public string SizeType { get; set; }

        /////////////////////////////////////////////////////////  
        
        [DisplayName("Blog")]
        public Guid BlogId { get; set; }
        
        public virtual Blog Blog { get; set; }
    }
}
