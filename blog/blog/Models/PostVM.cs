using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace blog.Models
{
    public class PostVM
    {
        public class PostCreateVM
        {
            public int PostId { get; set; }

            [Required]
            public string Title { get; set; }

            [Required]
            [AllowHtml]
            public string BlogContent { get; set; }

            public string Email { get; set; }

            [Required]
            public List<string> NameTag { get; set; }

            public byte[] Image { get; set; }

            public byte[] RowVersion { get; set; }
        }
        public class PostIndexVM
        {
            public int PostId { get; set; }

            public int UserId { get; set; }

            public string Title { get; set; }

            [AllowHtml]
            public string Description { get; set; }

            public string Slug { get; set; }

            public List<string> NameTag { get; set; }

            public string Image { get; set; }

            public string ImageUser { get; set; }

            public string Email { get; set; }

            public string AuthorName { get; set; }

            public string CreateDate { get; set; }

            public List<TagVM> ListTag { get; set; }

            public bool IsMySelf { get; set; }
        }
        public class PostDetailVM
        {
            public int PostId { get; set; }

            public string Title { get; set; }

            public string Slug { get; set; }

            public string BlogContent { get; set; }

            public string AuthorName { get; set; }

            public List<TagVM> ListTag { get; set; }

            public string Image { get; set; }
  
            public string CreateDate { get; set; }

            public bool IsMySelf { get; set; }

            public List<CommentVM> ListComment { get; set; }

        }

    }
}