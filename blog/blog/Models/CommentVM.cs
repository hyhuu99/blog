using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace blog.Models
{
    public class CommentVM
    {
        public int ID { get; set; }

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment field must be atleast 1000 characters")]
        public string CommentContent { get; set; }
        public DateTime? CreateDate { get; set; }
        public int PostId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ImageUser { get; set; }
        public bool IsMySelf { get; set; }
    }
}