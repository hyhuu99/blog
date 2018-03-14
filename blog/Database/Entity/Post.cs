using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    [Table("Post")]
    public class Post
    {
        public Post()
        {          
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
        }
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string BlogContent { get; set; }

        public string Slug { get; set; }

        public byte[] Image { get; set; }

        public DateTime? CreateDate { get; set; }

        public int Status { get; set; }

        public byte[] RowVersion { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

    }
}
