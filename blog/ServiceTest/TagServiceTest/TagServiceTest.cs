using AutoMapper;
using Database;
using Database.Repository;
using Database.UnitOfWork;
using DTO;
using Moq;
using Services;
using Services.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace ServicesTest.TagServiceTest
{
    public class TagServiceTest
    {
        Mock<IGenericRepository<Tag>> mockTagRepository;
        Mock<IUnitOfWork> mockUnitOfWork;
        TagService tagService;
        public TagServiceTest()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ServicesMappingProfile>();
            });
            mockTagRepository = new Mock<IGenericRepository<Tag>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            tagService = new TagService(mockTagRepository.Object, mockUnitOfWork.Object);
        }

        [Fact]
        public void IsExistsTag_should_be_return_true_if_tag_exists()
        {
            // Arrange
            string name = "test";
            Tag tag = new Tag();
            tag.Name = name;
            tag.Slug = name;
            var listTag = new List<Tag>();
            listTag.Add(tag);
            mockTagRepository.Setup(c => c.GetAll())
                .Returns(listTag.AsQueryable());
            //Act
            bool result = tagService.IsExistsTag(name);
            //Assert  
            Assert.Equal(true, result);
        }
        [Fact]
        public void IsExistsTag_should_be_return_false_if_tag_not_exists()
        {
            // Arrange
            string name = "test";
            Tag tag = new Tag();
            var listTag = new List<Tag>();
            listTag.Add(tag);
            mockTagRepository.Setup(c => c.GetAll())
                .Returns(listTag.AsQueryable());
            //Act
            bool result = tagService.IsExistsTag(name);
            //Assert  
            Assert.Equal(false, result);
        }

        [Fact]
        public void GetRamdom_should_be_null_if_tag_null()
        {
            // Arrange
            Tag tag = new Tag();
            var listTag = new List<Tag>();
            List<TagDTO> listtagDTO = new List<TagDTO>();
            mockTagRepository.Setup(c => c.GetAll())
                .Returns(listTag.AsQueryable());
            //Act
            List<TagDTO> result = tagService.GetRamdom();
            //Assert  
            Assert.Equal(listtagDTO, result);
        }

        [Fact]
        public void GetRamdom_should_be_call_if_tag_exists()
        {
            // Arrange
            Tag tag = new Tag();
            tag.Name = "test";
            var listTag = new List<Tag>();
            listTag.Add(tag);
            mockTagRepository.Setup(c => c.GetAll())
                .Returns(listTag.AsQueryable());
            //Act
            List<TagDTO> result = tagService.GetRamdom();
            //Assert  
            mockTagRepository.Verify(c => c.Search(It.IsAny<Expression<Func<Tag, bool>>>()));
        }

        [Fact]
        public void AutoCompleteTag_should_be_call_if()
        {
            // Arrange
            string name = "test";
            //Act
            List<TagDTO> result = tagService.AutoCompleteTag(name);
            //Assert  
            mockTagRepository.Verify(c => c.Search(It.IsAny<Expression<Func<Tag, bool>>>()));
        }

        [Fact]
        public void FindFormTag_should_be_return_null_if_tag_not_exists()
        {
            // Arrange
            List<PostDTO> listPostDTO = new List<PostDTO>();
            var listTag = new List<Tag>();
            mockTagRepository.Setup(c => c.Search(It.IsAny<Expression<Func<Tag, bool>>>()))
                .Returns(listTag.AsQueryable());            
            //Act
            List<PostDTO> result = tagService.FindFormTag(1, "1", 1, 5);
            //Assert  
            Assert.Equal(listPostDTO, result);
        }

        [Fact]
        public void FindFormTag_should_be_call_if_tag_exists()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Post post = new Post();
            post.BlogContent = "test";
            post.CreateDate = date;
            User user = new User() { ID = 1 };
            post.User = user;
            List<Post> listPost = new List<Post>();           
            listPost.Add(post);
            Tag tag = new Tag();
            tag.ID = 1;
            tag.Name = "test";
            tag.Slug = "1";
            tag.Posts = listPost;
            var listTag = new List<Tag>();
            listTag.Add(tag);
            mockTagRepository.Setup(c => c.Search(It.IsAny<Expression<Func<Tag, bool>>>()))
                .Returns(listTag.AsQueryable());
            //Act
            List<PostDTO> result = tagService.FindFormTag(1, "1", 0, 5);
            //Assert  
            Assert.IsType(typeof(List<PostDTO>), result);
        }

        [Fact]
        public void CountTags_should_be_return_zero_if_tag_null()
        {
            // Arrange
            var listTag = new List<Tag>();
            mockTagRepository.Setup(c => c.Search(It.IsAny<Expression<Func<Tag, bool>>>()))
                .Returns(listTag.AsQueryable());
            //Act
            int result = tagService.CountTags(1);
            //Assert  
            Assert.Equal(0, result);
        }

        [Fact]
        public void CountTags_should_be_return_1_if_tag_not_null()
        {
            // Arrange
            Tag tag = new Tag();
            tag.Name = "test";
            tag.ID = 1;
            Post post = new Post();
            post.BlogContent = "test";
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            tag.Posts = listPost;
            var listTag = new List<Tag>();
            listTag.Add(tag);
            mockTagRepository.Setup(c => c.Search(It.IsAny<Expression<Func<Tag, bool>>>()))
                .Returns(listTag.AsQueryable());
            //Act
            int result = tagService.CountTags(1);
            //Assert  
            Assert.Equal(1, result);
        }

    }
}
