using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nwBlog.Entities
{
    [Table("SentMails")]
    public class SentMail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public string Username { get; set; }

    }
}
