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
    [Table("Logs")]
    public class Log
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Kullanıcı"), StringLength(30)]
        public string Username { get; set; }

        [DisplayName("Action Name"), StringLength(50)]
        public string ActionName { get; set; }

        [DisplayName("Controller Name"), StringLength(50)]
        public string ControllerName { get; set; }

        [DisplayName("Content"), StringLength(500)]
        public string Content { get; set; }

        [DisplayName("Tarih")]
        public DateTime DateTime { get; set; }
    }
}
