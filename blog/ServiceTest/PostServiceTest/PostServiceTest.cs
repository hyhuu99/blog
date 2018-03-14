using Moq;
using Xunit;
using Services.Interface;
using Database.Repository;
using Database;
using Database.UnitOfWork;
using Services;
using AutoMapper;
using Services.AutoMapper;
using System.Collections.Generic;
using System.Linq;
using DTO;
using System;
using System.Linq.Expressions;
using Database;
using Database.Code;

namespace ServicesTest.PostServiceTest
{
    public class PostServiceTest
    {
        Mock<IGenericRepository<Post>> mockPostRepository;
        Mock<IGenericRepository<Tag>> mockTagRepository;
        Mock<IGenericRepository<User>> mockUserRepository;
        Mock<IUnitOfWork> mockUnitOfWork;
        UserService userService;
        PostService postService;
        public PostServiceTest()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ServicesMappingProfile>();
            });
            mockPostRepository = new Mock<IGenericRepository<Post>>();
            mockTagRepository = new Mock<IGenericRepository<Tag>>();
            mockUserRepository = new Mock<IGenericRepository<User>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            userService = new UserService(mockUserRepository.Object, mockUnitOfWork.Object);
            postService = new PostService(mockPostRepository.Object, mockTagRepository.Object, userService, mockUnitOfWork.Object);
        }
        [Fact]
        public void IsCreateSuccessful_should_be_return_false_if_postDTO_not_exists()
        {
            // Arrange
            PostDTO postDTO=null;         
            //Act
            bool result = postService.IsCreateSuccessful(postDTO);
            //Assert  
            Assert.Equal(false, result);
        }
        [Fact]
        public void IsCreateSuccessful_should_be_call_if_postDTO_exists()
        {
            // Arrange
            PostDTO postDTO = new PostDTO();
            postDTO.Title = "test";
            postDTO.NameTag = new List<string> { "test1", "test2" };
            //Act
            bool result = postService.IsCreateSuccessful(postDTO);
            //Assert  
            mockPostRepository.Verify(c => c.Insert(It.IsAny<Post>()));
        }
        [Fact]
        public void LoadPosts_should_be_return_listpost()
        {
            // Arrange
            PostDTO postDTO = new PostDTO();
            postDTO.Title = "test";
            postDTO.NameTag = new List<string> { "test1", "test2" };
            //Act
            List<PostDTO> result = postService.LoadPosts(0,5);
            //Assert  
            Assert.IsType(typeof(List<PostDTO>), result);
        }
        [Fact]
        public void FindPost_should_be_return_null_if_post_not_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            Post post = new Post();
            post.ID = 0;
            post.Title = "test";
            post.Slug = "test";
            post.User = new User() { Email = email };
            post.Comments = new List<Comment>();
            post.Tags = new List<Tag>();
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            PostDTO result = postService.FindPost(1, "test");
            //Assert  
            Assert.Equal(null, result);
        }

        [Fact]
        public void FindPost_should_be_return_post()
        {
            // Arrange
            string email = "admin@gmail.com";
            Post post = new Post();
            post.ID = 1;
            post.Title = "test";
            post.Slug = "test";
            post.User = new User() { Email = email };
            post.Comments = new List<Comment>();
            post.Tags = new List<Tag>();
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            PostDTO result = postService.FindPost(1,"test");
            //Assert  
            Assert.IsType(typeof(PostDTO), result);
        }

        [Fact]
        public void IsUpdateSuccessful_should_be_return_fail_if_post_not_exists()
        {
            // Arrange
            PostDTO postDTO = new PostDTO();
            postDTO.ID = 1;          
            List<Post> listPost = new List<Post>();
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            int result = postService.IsUpdateSuccessful(postDTO);
            //Assert  
            Assert.Equal((int)StatusCode.FAIL, result);
        }

        [Fact]
        public void IsUpdateSuccessful_should_be_call_if_post_exists()
        {
            // Arrange
            string email = "admin@gmail.com";        
            PostDTO postDTO = new PostDTO();
            postDTO.ID = 1;
            postDTO.Email = email;
            postDTO.Title = "test";
            postDTO.NameTag = new List<string>()
            {
                "test","sum"
            };
            Post post = new Post();
            post.ID = 1;
            post.Title = "test";
            post.Slug = "test";
            post.User = new User() { Email = email };
            post.Comments = new List<Comment>();
            post.Tags = new List<Tag>();
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            int result = postService.IsUpdateSuccessful(postDTO);
            //Assert  
            mockPostRepository.Verify(c => c.Update(It.IsAny<Post>()));
        }

        [Fact]
        public void IsMySelft_should_be_return_false_if_post_not_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            List<Post> listPost = new List<Post>();
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            bool result = postService.IsMySelft(email,1);
            //Assert  
            Assert.Equal(false ,result);
        }

        [Fact]
        public void IsMySelft_should_be_return_true_if_post_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            Post post = new Post();
            post.ID = 1;
            post.User = new User() { Email = email };
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            bool result = postService.IsMySelft(email, 1);
            //Assert  
            Assert.Equal(true, result);
        }


        [Fact]
        public void AutoCompleSearch_should_be_call()
        {
            // Arrange
            Post post = new Post();
            post.ID = 1;
            post.Title = "test";
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            List<PostDTO> result = postService.AutoCompleSearch("t");
            //Assert  
            Assert.IsType(typeof(List<PostDTO>), result);
        }

        [Fact]
        public void IsDeleteSuccessful_should_be_return_false_if_post_not_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            List<Post> listPost = new List<Post>();
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            bool result = postService.IsDeleteSuccessful(email,1);
            //Assert  
            Assert.Equal(false, result);
        }
        [Fact]
        public void IsDeleteSuccessful_should_be_call_if_post_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            Post post = new Post();
            post.ID = 1;
            post.Title = "test";
            post.User = new User() { Email = email };
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            bool result = postService.IsDeleteSuccessful(email, 1);
            //Assert  
            mockPostRepository.Verify(c => c.Delete(It.IsAny<Post>()));
        }

        [Fact]
        public void GetPersonalPage_should_be_call_if_email_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            Post post = new Post();
            post.ID = 1;
            post.Title = "test";
            post.User = new User() { Email = email };
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            List<PostDTO> result = postService.GetPersonalPage(0,email,0,5);
            //Assert  
            Assert.IsType(typeof(List<PostDTO>), result);
        }

        [Fact]
        public void GetPersonalPage_should_be_call_if_userid_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            Post post = new Post();
            post.ID = 1;
            post.Title = "test";
            post.User = new User() { Email = email ,ID=1};
            List<Post> listPost = new List<Post>();
            listPost.Add(post);
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            List<PostDTO> result = postService.GetPersonalPage(1, null, 0, 5);
            //Assert  
            Assert.IsType(typeof(List<PostDTO>), result);
        }

        [Fact]
        public void GetTotalCount_should_be_call_exactly()
        {
            // Arrange
            List<Post> listPost = new List<Post>()
            {
                new Post() {ID=1,Title="test1"},
                new Post() {ID=2,Title="test2"}
            };
            mockPostRepository.Setup(y => y.GetAll())
                .Returns(listPost.AsQueryable());
            //Act
            int result = postService.GetTotalCount();
            //Assert  
            Assert.Equal(2,result);
        }

        [Fact]
        public void GetCountPostInUser_should_be_call_exactly()
        {
            // Arrange
            string email = "admin@gmail.com";
            User user = new User();
            user.ID = 1;
            user.Email = email;
            List<Post> listPost = new List<Post>()
            {
                new Post() {ID=1,Title="test1",User=user},
                new Post() {ID=2,Title="test2",User=user}
            };
            mockPostRepository.Setup(c => c.Search(It.IsAny<Expression<Func<Post,bool>>>()))
                .Returns(listPost.AsQueryable());
            //Act
            int result = postService.GetCountPostInUser(email);
            //Assert  
            Assert.Equal(2, result);
        }
    }
}
