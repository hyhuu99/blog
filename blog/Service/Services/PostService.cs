using AutoMapper;
using Database;
using Database.Code;
using Database.Repository;
using Database.UnitOfWork;
using DTO;
using HtmlAgilityPack;
using Utility.AnotherFunction;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Entity;

namespace Services
{
    public class PostService:IPostService
    {
        readonly IGenericRepository<Post> _post;
        readonly IGenericRepository<Tag> _tag;
        readonly IUserService _user;
        readonly IUnitOfWork _unitOfWork;
        public PostService(IGenericRepository<Post> postGeneric, IGenericRepository<Tag> tagGeneric, IUserService userService, IUnitOfWork unitOfWork)
        {
            _post = postGeneric;
            _tag = tagGeneric;
            _user = userService;
            _unitOfWork = unitOfWork;
        }

        public bool IsCreateSuccessful(PostDTO postModel)
        {
            if(postModel!=null)
            {              
                List<Tag> listTag = InsertTag(postModel);
                InsertPost(postModel, listTag);
                _unitOfWork.Commit();
                return true;
            }
            return false;
        }
       
        private void InsertPost(PostDTO postModel, List<Tag> listTag)
        {
            Post post = Mapper.Map<PostDTO, Post>(postModel);
            post.Description = ConvertDescription(post.BlogContent);
            post.Tags = listTag;
            post.Slug = ConvertToUrlSlug(postModel.Title);
            post.User = _user.GetUser(postModel.Email);
            _post.Insert(post);
        }     
        public List<PostDTO> LoadPosts(int page, int itemsPerPage)
        {
            List<Post> listPosts = _post.GetAll().Include(c => c.User).Include(c => c.Tags).OrderByDescending(c => c.CreateDate).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            List<PostDTO> listPostsDto = new List<PostDTO>();
            foreach (Post post in listPosts)
            {
                PostDTO postDto = Mapper.Map<Post, PostDTO>(post);
                postDto.UserId = post.User.ID;
                postDto.ImageUser = post.User.ImageUser;
                postDto.AuthorName = post.User.DisplayName;
                postDto.CreateDate = post.CreateDate.Value.ToString("dd-MM-yyyy");
                postDto.ListTagDTO = Mapper.Map<List<Tag>, List<TagDTO>>(post.Tags.ToList());
                listPostsDto.Add(postDto);
            }
            return listPostsDto;
        }

        public int GetTotalCount()
        {
            return _post.GetAll().Count();
        }
        public List<PostDTO> AutoCompleSearch(string value)
        {
            List<Post> listPost = _post.Search(c => c.Title.Contains(value)).ToList();
            List<PostDTO> listPostsDTO = new List<PostDTO>();
            foreach (Post post in listPost)
            {
                PostDTO postDTO = Mapper.Map<Post,PostDTO>(post);           
                listPostsDTO.Add(postDTO);
            }
            return listPostsDTO;
        }

        public PostDTO FindPost(int id,string slug)
        {
            Post post = _post.GetAll().Include(c => c.User).Include(c => c.Comments).Include(c => c.Tags).FirstOrDefault(c => c.ID==id && c.Slug == slug);
            PostDTO postDto = Mapper.Map<Post, PostDTO>(post);
            if (post!=null)
            {               
                postDto.AuthorName = post.User.DisplayName;
                postDto.NameTag = ConvertListTag(post.Tags.ToList());
                postDto.ListTagDTO = Mapper.Map<List<Tag>, List<TagDTO>>(post.Tags.ToList());
                postDto.ListCommentDTO = MappingComment(post.Comments.ToList());
            }
            return postDto;
        }

        public int IsUpdateSuccessful(PostDTO postModel)
        {
            Post post = _post.GetAll().Include(c => c.Tags).FirstOrDefault(c => c.ID == postModel.ID && c.User.Email == postModel.Email);
            if (post != null)
            {
                
                List<Tag> listTag = UpdateTag(post.Tags.ToList(),postModel);
                post.Title = postModel.Title;
                post.Slug = ConvertToUrlSlug(postModel.Title);
                post.BlogContent = postModel.BlogContent;
                post.Description = ConvertDescription(postModel.BlogContent);

                if(postModel.Image!=null)
                {
                    post.Image = postModel.Image;
                }             
                 post.Tags = listTag;                                 
                _post.EditsConcurrency(post, postModel.RowVersion);
                _post.Update(post);
                return _unitOfWork.SpecialCommit();
            }
            return (int)StatusCode.FAIL;
        }
        public bool IsMySelft(string email,int postId)
        {
            Post post= _post.GetAll().FirstOrDefault(c => c.User.Email == email && c.ID == postId);
            if (post != null)
                return true;
            return false;
        }
        public bool IsDeleteSuccessful(string email,int postId)
        {
            Post post = _post.GetAll().Include(c => c.Tags).FirstOrDefault(c => c.User.Email == email && c.ID == postId);
            if (post == null)
                return false;
            DeleteTag(post.Tags.ToList());
            _post.Delete(post);
            return _unitOfWork.Commit();
        }
        public List<PostDTO> GetPersonalPage(int userId, string email, int page, int itemsPerPage)
        {
            List<Post> listPost;
            if (userId == 0 && !string.IsNullOrEmpty(email))
            {
                listPost = _post.Search(c => c.User.Email == email).ToList();
            }
            else
            {
                listPost = _post.Search(c => c.User.ID == userId).ToList();
            }
            List<Post> result = listPost.OrderByDescending(c => c.CreateDate).Skip(itemsPerPage * page).Take(itemsPerPage).ToList();
            List<PostDTO> listPostsDto = new List<PostDTO>();
            foreach (Post post in result)
            {
                PostDTO postDto = Mapper.Map<Post, PostDTO>(post);
                postDto.Email = post.User.Email;
                postDto.ImageUser = post.User.ImageUser;
                postDto.AuthorName = post.User.DisplayName;
                postDto.CreateDate = post.CreateDate.Value.ToString("dd-MM-yyyy");
                postDto.ListTagDTO = Mapper.Map<List<Tag>, List<TagDTO>>(post.Tags.ToList());
                listPostsDto.Add(postDto);
            }
            return listPostsDto;
        }
        private List<CommentDTO> MappingComment(List<Comment> listComment)
        {
            List<CommentDTO> listCommentDto = new List<CommentDTO>();
            foreach (Comment comment in listComment)
            {
                CommentDTO commentDto = Mapper.Map<Comment, CommentDTO>(comment);
                commentDto.DisplayName = comment.User.DisplayName;
                commentDto.ImageUser = ImageFunction.ConvertImage(comment.User.ImageUser);
                commentDto.Email = comment.User.Email;
                listCommentDto.Add(commentDto);
            }
            return listCommentDto;
        }
        private List<string> ConvertListTag(List<Tag> listTag)
        {
            List<string> listNameTag = new List<string>();
            if (listTag!=null)
            {
                foreach (Tag tag in listTag)
                {
                    listNameTag.Add(tag.Name);
                }
                return listNameTag;
            }
            return listNameTag;
        }
        private string ConvertToUrlSlug(string value)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(value);
            var textString = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
            value = textString;

            value = value.ToLowerInvariant();

            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            value = value.Trim('-', '_');

            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);
            //value += phrase;

            return value;
        }

        private string ConvertDescription(string text)
        {
            
            string result = "";
            if (!string.IsNullOrEmpty(text))
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(text);
                var textString = HtmlEntity.DeEntitize(doc.DocumentNode.InnerText);
                text = textString;
                int length = text.Length < 200 ? text.Length : 200;
                result= text.Substring(0, text.IndexOf(" ", length, StringComparison.Ordinal));
            }
            result += "...";
            return result;
        }
       
        private void UpdatePost(PostDTO postModel, List<Tag> listTag)
        {
            Post post;
            post = Mapper.Map<PostDTO, Post>(postModel);
            post.Description = ConvertDescription(post.BlogContent);
            post.Status = (int)StatusCode.ACTIVE;
            post.CreateDate = DateTime.Now;
            post.Tags = listTag;
            post.Slug = ConvertToUrlSlug(postModel.Title);
            _post.Insert(post);
        }


        private List<Tag> InsertTag(PostDTO postModel)
        {
            List<Tag> listTag = new List<Tag>();
            foreach (string name in postModel.NameTag)
            {
                Tag tag = new Tag();
                if (IsExistsTag(name))
                {
                    tag = UpdateTag(name);
                }
                else
                {
                    tag.Name = name.ToLower();
                    tag.Slug = ConvertToUrlSlug(tag.Name);
                    tag.Frequency = 1;
                    _tag.Insert(tag);
                }
                listTag.Add(tag);
            }
            return listTag;
        }
        private Tag UpdateTag(string name)
        {
            Tag tag = _tag.GetAll().FirstOrDefault(c => c.Name == name.ToLower());
            tag.Frequency += 1;
            _tag.Update(tag);
            return tag;
        }

        private List<Tag> UpdateTag(List<Tag> listTag, PostDTO postModel)
        {
            List<Tag> _listTag = new List<Tag>();
            foreach (Tag tag in listTag)
            {
                if (IsExistsTag(tag.Name) && tag.Frequency > 0)
                {
                    tag.Frequency -= 1;
                    _tag.Update(tag);
                }
            }
            foreach (string name in postModel.NameTag)
            {

                Tag tag = new Tag();
                if (IsExistsTag(name))
                {
                    tag = _tag.GetAll().FirstOrDefault(c => c.Name == name.ToLower());
                    tag.Frequency += 1;
                    _tag.Update(tag);
                }
                else
                {
                    tag.Name = name.ToLower();
                    tag.Slug = ConvertToUrlSlug(tag.Name);
                    tag.Frequency = 1;
                    _tag.Insert(tag);
                }
                _listTag.Add(tag);
            }
            return _listTag;
        }
        private bool IsExistsTag(string name)
        {
            Tag tag = _tag.GetAll().FirstOrDefault(c => c.Name == name.ToLower());
            return tag != null ? true : false;
        }
        private void DeleteTag(List<Tag> listTag)
        {
            foreach (Tag tag in listTag)
            {
                if (IsExistsTag(tag.Name) && tag.Frequency > 0)
                {
                    tag.Frequency -= 1;
                    _tag.Update(tag);
                }
            }
        }

        public int GetCountPostInUser(string email)
        {
            return _post.Search(c => c.User.Email == email).ToList().Count;
        }
    }
}
