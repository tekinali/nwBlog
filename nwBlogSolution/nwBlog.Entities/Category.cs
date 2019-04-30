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
    [Table("Categories")]
    public class Category : MyEntityBase<int>
    {
        [DisplayName("Kategori"), Required, StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Açıklama"), StringLength(250)]
        public string Description { get; set; }

        public virtual List<BlogCategory> BlogCategories { get; set; }

        public Category()
        {
            BlogCategories = new List<BlogCategory>();
        }

    }
}
