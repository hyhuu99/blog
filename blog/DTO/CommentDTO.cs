using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CommentDTO
    {
        public int ID { get; set; }
        public string CommentContent { get; set; }
        public DateTime? CreateDate { get; set; }
        public int PostId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ImageUser { get; set; }
        public bool IsMySelf { get; set; }
    }
}
