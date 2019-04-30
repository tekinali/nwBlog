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
    [Table("AppUsers")]
    public class AppUser : MyEntityBase<Guid>
    {
        [DisplayName("Ad"), Required, StringLength(30)]
        public string Name { get; set; }

        [DisplayName("Soyad"), Required, StringLength(30)]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"), Required, StringLength(20)]
        public string Username { get; set; }

        [DisplayName("Hakkımda"), StringLength(500)]
        public string About { get; set; }

        [DisplayName("E-posta"), Required, DataType(DataType.EmailAddress), StringLength(50)]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required, DataType(DataType.Password), StringLength(100)]
        public string Password { get; set; }

        [StringLength(30), ScaffoldColumn(false)]
        public string ProfileImageFilename { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Delete")]
        public bool IsDeleted { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        [DisplayName("Şehir")]
        public int CityId { get; set; }

        ///////////////////////////////////////////////////

        public virtual City City { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Blog> Blogs { get; set; }
        public virtual List<Like> Likes { get; set; }
        public virtual List<LastVisit> LastVisits { get; set; }

        public AppUser()
        {
            UserRoles = new List<UserRole>();
            Comments = new List<Comment>();
            Blogs = new List<Blog>();
            Likes = new List<Like>();
            LastVisits = new List<LastVisit>();
        }



    }
}
