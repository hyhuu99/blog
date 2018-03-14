using AutoMapper;
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
namespace ServicesTest.UserServiceTest
{
    public class UserServiceTest
    {
        Mock<IGenericRepository<Database.User>> mockUserRepository;
        Mock<IUnitOfWork> mockUnitOfWork;
        UserService userService;
        public UserServiceTest()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ServicesMappingProfile>();
            });
            mockUserRepository = new Mock<IGenericRepository<Database.User>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            userService = new UserService(mockUserRepository.Object, mockUnitOfWork.Object);
        }

        [Fact]
        public void IsLoginSuccessful_should_be_call_if_user_exists()
        {
            // Arrange
            UserDTO userDTO = new UserDTO();
            userDTO.Email = "admin@gmail.com";
            userDTO.Password = "a0c1d878ea1ad8761dbc12e3f33836e0";
            Database.User user = new Database.User();
            user.Email = "admin@gmail.com";
            user.Password = "a0c1d878ea1ad8761dbc12e3f33836e0";
            var listUser = new List<Database.User>();
            listUser.Add(user);
            mockUserRepository.Setup(y => y.Search(It.IsAny<Expression<Func<Database.User, bool>>>()))
                .Returns(listUser.AsQueryable());
            //Act
            bool result = userService.IsLoginSuccessful(userDTO);
            //Assert         
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsLoginSuccessful_should_not_be_login_if_user_not_exists()
        {
            // Arrange
            UserDTO userDTO = new UserDTO();
            userDTO.Email = "admin@gmail.com";
            userDTO.Password = "a0c1d878ea1ad8761dbc12e3f33836e0";
            List<Database.User> listUser = new List<Database.User>();
            mockUserRepository.Setup(y => y.Search(It.IsAny<Expression<Func<Database.User, bool>>>()))
                .Returns(listUser.AsQueryable());
            //Act
            bool result = userService.IsLoginSuccessful(userDTO);

            //Assert        
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsEmailExists_should_be_call_if_email_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            UserDTO userDTO = new UserDTO();
            userDTO.Email = email;
            Database.User user = new Database.User();
            user.Email = email;
            var listUser = new List<Database.User>();
            listUser.Add(user);
            mockUserRepository.Setup(y => y.Search(It.IsAny<Expression<Func<Database.User, bool>>>()))
                .Returns(listUser.AsQueryable());
            //Act
            bool result = userService.IsEmailExists(userDTO.Email);
            //Assert         
            Assert.Equal(true, result);
        }

        [Fact]
        public void IsEmailExists_should_be_call_if_email_not_exists()
        {
            // Arrange
            string email = "admin@gmail.com";
            UserDTO userDTO = new UserDTO();
            userDTO.Email = email;
            List<Database.User> listUser = new List<Database.User>();          
            mockUserRepository.Setup(y => y.Search(It.IsAny<Expression<Func<Database.User, bool>>>()))
                .Returns(listUser.AsQueryable());
            //Act
            bool result = userService.IsEmailExists(email);

            //Assert        
            Assert.Equal(false, result);
        }

        [Fact]
        public void Register_user_should_be_call()
        {
            // Arrange
            UserDTO user = new UserDTO();
            user.ID = 1;
            user.Email = "admin@gmail.com";
            user.Password = "a0c1d878ea1ad8761dbc12e3f33836e0";
            var mockUserRepository = new Mock<IGenericRepository<Database.User>>();
          
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var userService = new UserService(mockUserRepository.Object, mockUnitOfWork.Object);
            mockUnitOfWork.Setup(y => y.Commit());
            //Act
            int? result = userService.Register(user);
            mockUserRepository.Verify(y => y.Insert(It.IsAny<Database.User>()));
            //Assert
            Assert.Equal(1, result);
        }
        [Fact]
        public void GetUserDTO_user_should_be_call()
        {
            // Arrange
            UserDTO userDTO = new UserDTO();
            userDTO.Email = "admin@gmail.com";              
            //Act
            UserDTO result = userService.GetUserDTO(userDTO.Email);

            //Assert
            mockUserRepository.Verify(c => c.GetAll());
        }

        [Fact]
        public void GetUser_user_should_be_call()
        {
            // Arrange
            string email = "admin@gmail.com";
         
            //Act
            Database.User result = userService.GetUser(email);

            //Assert
            mockUserRepository.Verify(c => c.GetAll());
        }
        [Fact]
        public void IsUpdateSuccessful_if_user_null__return_false()
        {
            // Arrange
            UserDTO userDTO = new UserDTO();
            userDTO.Email = "admin@gmail.com";
            userDTO.Password = "1";
            Database.User user = new Database.User();
            user.Email = "admin@gmail.com";
            user.Password = "1";
            List<Database.User> listUser = new List<Database.User>();
            listUser.Add(user);         
            mockUserRepository.Setup(c => c.GetAll())
                .Returns(listUser.AsQueryable());
            //Act
            bool result = userService.IsUpdateSuccessful(userDTO);

            //Assert
            Assert.Equal(false, result);
        }

        [Fact]
        public void IsUpdateSuccessful_should_be_call_if_user_exists()
        {
            // Arrange
            UserDTO userDTO = new UserDTO();
            userDTO.Email = "admin@gmail.com";
            userDTO.Password = "1";
            Database.User user = new Database.User();
            user.Email = "admin@gmail.com";
            user.Password = "1";
            List<Database.User> listUser = new List<Database.User>();
            listUser.Add(user);       
            mockUserRepository.Setup(c => c.GetAll())
                .Returns(listUser.AsQueryable());
            mockUnitOfWork.Setup(c => c.Commit()).Returns(true);
            //Act
            bool result = userService.IsUpdateSuccessful(userDTO);

            //Assert
            Assert.Equal(true, result);
        }
    }
}
