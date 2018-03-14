using Database;
using DTO;
using System.Collections.Generic;

namespace Services.Interface
{
    public interface ITagService
    {
        
        bool IsExistsTag(string name);
        List<TagDTO> GetRamdom();
        List<TagDTO> AutoCompleteTag(string value);
        List<PostDTO> FindFormTag(int id, string slug, int page, int itemsPerPage);
        int CountTags(int id);
    }
}
