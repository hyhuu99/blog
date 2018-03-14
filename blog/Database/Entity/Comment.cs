using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public int ID { get; set; }

        public string CommentContent { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual User User { get; set; }
    }
}
