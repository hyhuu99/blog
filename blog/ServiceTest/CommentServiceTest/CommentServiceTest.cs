using AutoMapper;
using Database;
using Database.Repository;
using Database.UnitOfWork;
using DTO;
using Moq;
using Services;
using Services.AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ServicesTest.CommentServiceTest
{
    public class CommentServiceTest
    {
        Mock<IGenericRepository<Comment>> mockCommentRepository;
        Mock<IGenericRepository<Post>> mockPostRepository;
        Mock<IGenericRepository<User>> mockUserRepository;
        Mock<IUnitOfWork> mockUnitOfWork;
        CommentService commentService;
        public CommentServiceTest()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ServicesMappingProfile>();
            });
            mockCommentRepository = new Mock<IGenericRepository<Comment>>();
            mockPostRepository = new Mock<IGenericRepository<Post>>();
            mockUserRepository = new Mock<IGenericRepository<User>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            commentService = new CommentService(mockCommentRepository.Object, mockPostRepository.Object, mockUserRepository.Object, mockUnitOfWork.Object);
        }
        [Fact]
        public void IsDeleteSuccessful_should_be_return_false_if_comment_not_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            List<Comment> listComment = new List<Comment>();
            mockCommentRepository.Setup(c => c.GetAll())
                .Returns(listComment.AsQueryable());
            //Act
            bool result = commentService.IsDeleteSuccessful(1, email);
            //Assert  
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsDeleteSuccessful_should_be_call_if_comment_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            User user = new User();
            user.Email = email;
            Comment comment = new Comment();
            comment.ID = 1;
            comment.User = user;
            List<Comment> listComment = new List<Comment>();
            listComment.Add(comment);
            mockCommentRepository.Setup(c => c.GetAll())
                .Returns(listComment.AsQueryable());
            //Act
            bool result = commentService.IsDeleteSuccessful(1, email);
            //Assert  
            mockCommentRepository.Verify(y => y.Delete(It.IsAny<Comment>()));
        }

        [Fact]
        public void IsCreateSuccessful_should_be_return_0_if_commentdto_not_exists()
        {
            // Arrange
            CommentDTO commentDTO = new CommentDTO();
            Post post = new Post();
            List<User> listUser = new List<User>();
            mockPostRepository.Setup(c => c.GetById(It.IsAny<int?>()))
                .Returns(post);
            mockUserRepository.Setup(c => c.GetAll())
                .Returns(listUser.AsQueryable());
            //Act
            int? result = commentService.IsCreateSuccessful(commentDTO);
            //Assert  
            Assert.Equal(0, result);
        }

        [Fact]
        public void IsCreateSuccessful_should_be_return_1_if_commentdto_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            CommentDTO commentDTO = new CommentDTO();
            commentDTO.ID = 1;
            commentDTO.Email = email;
            Post post = new Post();
            post.ID = 1;
            User user = new User();
            user.Email = email;
            List<User> listUser = new List<User>();
            listUser.Add(user);
            mockPostRepository.Setup(c => c.GetById(It.IsAny<int?>()))
                .Returns(post);
            mockUserRepository.Setup(c => c.GetAll())
                .Returns(listUser.AsQueryable());
            //Act
            int? result = commentService.IsCreateSuccessful(commentDTO);
            //Assert  
            Assert.Equal(1, result);
        }


    }
}
