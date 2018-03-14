using System.Collections.Generic;

namespace DTO
{
    public class PostDTO
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string BlogContent { get; set; }     
        public string Email { get; set; }
      
        public byte[] Image { get; set; }
        public string AuthorName { get; set; }
        public string CreateDate { get; set; }
        public byte[] ImageUser { get; set; }
        public byte[] RowVersion { get; set; }
        public List<CommentDTO> ListCommentDTO { get; set; }
        public List<string> NameTag { get; set; }
        public List<TagDTO> ListTagDTO { get; set; }
    }
}
