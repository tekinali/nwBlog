using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace nwBlog.Entities
{
    [Table("Blogs")]
    public class Blog : MyEntityBase<Guid>
    {
        [DisplayName("Başlık"), Required, StringLength(250)]
        public string Tittle { get; set; }

        [DisplayName("Özet"), Required, StringLength(1000)]
        public string Summary { get; set; }

        
        [DisplayName("İçerik"), Required, StringLength(5000)]    
        [AllowHtml]
        public string Text { get; set; }

        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }

        [DisplayName("Sil")]
        public bool IsDelete { get; set; }

        [ScaffoldColumn(false), StringLength(20)]
        public string UrlName { get; set; }

        /////////////////////////////////////////////////////////  
     
        public Guid AppUserId { get; set; }

        public virtual AppUser AppUser { get; set; }

        public virtual List<BlogCategory> BlogCategories { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual List<Like> Likes { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<BlogImage> BlogImages { get; set; }
        
        public Blog()
        {
            BlogCategories = new List<BlogCategory>();
            Tags = new List<Tag>();
            Likes = new List<Like>();
            Comments = new List<Comment>();
            BlogImages = new List<BlogImage>();
        }

    }
}
