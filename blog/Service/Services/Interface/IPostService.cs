using DTO;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface IPostService
    {
        bool IsCreateSuccessful(PostDTO postModel);
        List<PostDTO> LoadPosts(int page, int pageSize);
        PostDTO FindPost(int id, string slug);
        int IsUpdateSuccessful(PostDTO postModel);
        bool IsMySelft(string email, int postId);
        List<PostDTO> AutoCompleSearch(string value);
        bool IsDeleteSuccessful(string email, int postId);
        List<PostDTO> GetPersonalPage(int userId, string email, int page, int itemsPerPage);
        int GetTotalCount();
        int GetCountPostInUser(string email);
    }
}
