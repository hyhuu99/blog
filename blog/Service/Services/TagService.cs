using AutoMapper;
using Database;
using Database.Repository;
using Database.UnitOfWork;
using DTO;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services
{
    public class TagService:ITagService
    {
        IGenericRepository<Tag> _tag;
        IUnitOfWork _unitOfWork;

        public TagService(IGenericRepository<Tag> tagGeneric, IUnitOfWork unitOfWork)
        { 
            _tag = tagGeneric;
            _unitOfWork = unitOfWork;
        }
        
        public bool IsExistsTag(string name)
        {
            Tag tag = _tag.GetAll().FirstOrDefault(c => c.Name == name.ToLower());
            return tag != null ? true : false;
        }
        public List<TagDTO> AutoCompleteTag(string value)
        {
            List<Tag> listTag = _tag.Search(c => c.Name.Contains(value)).ToList();
            List<TagDTO> listTagDTO = new List<TagDTO>();
            foreach (Tag tag in listTag)
            {
                TagDTO tagDTO = Mapper.Map<Tag, TagDTO>(tag);      
                listTagDTO.Add(tagDTO);
            }
            return listTagDTO;
        }

        public List<TagDTO> GetRamdom()
        {
            List<Tag> listTag = new List<Tag>();
            List<TagDTO> result = new List<TagDTO>();
            int totalCount = _tag.GetAll().Count();
            
            if (totalCount == 0)
                return result;
            int value = totalCount>5 ? 5 :totalCount;
            List<int> listInt = RandomList(value, totalCount);
            listTag = _tag.Search(c => listInt.Contains(c.ID)).ToList();
            result = Mapper.Map<List<Tag>, List<TagDTO>>(listTag);        
            return result;
        }
        private List<int> RandomList(int value,int totalCount)
        {
            
            List<int> listInt = new List<int>(totalCount);
            List<int> result = new List<int>(value);
            for (int i = 1; i <= totalCount; i++)
            {
                listInt.Add(i);
            }
            Random rand = new Random();
            while(value>0)
            {
                int index = rand.Next(1, listInt.Count+1);
                if (!result.Contains(index))
                {
                    result.Add(index);
                    value--;
                }
            }       
            return result;
        }
        public List<PostDTO> FindFormTag(int id,string slug, int page, int itemsPerPage)
        {
            Tag tag = _tag.Search(c => c.ID == id && c.Slug == slug).Include(c => c.Posts.Select(y => y.User))
                .Include(c => c.Posts.Select(y => y.Tags)).FirstOrDefault();
            
            List<PostDTO> listPostDTO = new List<PostDTO>();
            if (tag == null)
                return listPostDTO;
            foreach (Post post in tag.Posts.ToList())
            {
                    PostDTO postDTO = Mapper.Map<Post, PostDTO>(post);
                    postDTO.UserId = post.User.ID;
                    postDTO.ImageUser = post.User.ImageUser;
                    postDTO.AuthorName = post.User.DisplayName;
                    postDTO.CreateDate = post.CreateDate.Value.ToString("dd-MM-yyyy");
                    postDTO.ListTagDTO = Mapper.Map<List<Tag>, List<TagDTO>>(post.Tags.ToList());
                    listPostDTO.Add(postDTO);
            }
            List<PostDTO> result = listPostDTO.OrderByDescending(c => c.CreateDate).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            return result;
        }

        public int CountTags(int id)
        {
            int count = 0;
            Tag tag = _tag.Search(c => c.ID == id).Include(c => c.Posts).FirstOrDefault();
            if(tag!=null)
            {
                count = tag.Posts.ToList().Count();
            }
            return count;
        }
    }
}
