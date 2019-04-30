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
    [Table("Cities")]
    public class City : MyEntityBase<int>
    {
        [DisplayName("Şehir"), Required, StringLength(50)]
        public string Name { get; set; }

        public virtual List<AppUser> AppUsers { get; set; }

        public City()
        {
            AppUsers = new List<AppUser>();
        }
    }
}
