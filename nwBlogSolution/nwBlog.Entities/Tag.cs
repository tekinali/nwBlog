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
    [Table("Tags")]
    public class Tag : MyEntityBase<int>
    {
        [DisplayName("Etiket"), Required, StringLength(20)]
        public string Name { get; set; }

        public Guid BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
