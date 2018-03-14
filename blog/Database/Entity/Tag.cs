using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database
{
    [Table("Tag")]
    public class Tag
    {
        public Tag()
        {
            Posts = new HashSet<Post>();
        }

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public int Frequency { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
