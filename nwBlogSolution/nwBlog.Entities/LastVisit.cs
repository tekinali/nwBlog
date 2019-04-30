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
    [Table("LastVisits")]
    public class LastVisit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [DisplayName("Ip Adres"), StringLength(50)]
        public string IpAddress { get; set; }

        [DisplayName("Kullanıcı")]
        public Guid AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }

    }
}
