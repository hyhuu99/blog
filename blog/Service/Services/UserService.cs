using AutoMapper;
using Database;
using Database.Code;
using Database.Repository;
using Database.UnitOfWork;
using DTO;
using Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class UserService : IUserService
    {
        IGenericRepository<User> _user;
        IUnitOfWork _unitOfWork;
        public UserService(IGenericRepository<User> user, IUnitOfWork unitOfWork)
        {
            _user = user;
            _unitOfWork = unitOfWork;
        }
        public bool IsLoginSuccessful(UserDTO user)
        {
            List<User> ListUser = _user.Search(c => c.Email.Equals(user.Email) && c.Password.Equals(user.Password)).ToList();
            return ListUser.Any();
        }
        public bool IsEmailExists(string email)
        {
            List<User> ListUser = _user.Search(c => c.Email.Equals(email)).ToList();
            return ListUser.Any();
        }
        public int? Register(UserDTO user)
        {
            if (IsEmailExists(user.Email))
                return (int)StatusCode.ISEXISTSUSER;
            User userEntity = new User();
            userEntity = Mapper.Map<UserDTO, User>(user);
            _user.Insert(userEntity);
            _unitOfWork.Commit();
            return userEntity.ID;
        }

        public UserDTO GetUserDTO(string email)
        {
            User user = _user.GetAll().FirstOrDefault(c => c.Email.Equals(email));
            UserDTO userDTO = Mapper.Map<User, UserDTO>(user);
            return userDTO;
        }
        public User GetUser(string email)
        {
            User user = _user.GetAll().FirstOrDefault(c => c.Email.Equals(email) && c.Email == email);
            return user;
        }

        public bool IsUpdateSuccessful(UserDTO userDTO)
        {
            User user = _user.GetAll().FirstOrDefault(c => c.Email.Equals(userDTO.Email));
            if(user!=null)
            {
                user.DisplayName = userDTO.DisplayName;
                if(userDTO.ImageUser!=null)
                user.ImageUser = userDTO.ImageUser;
                _user.Update(user);
                return _unitOfWork.Commit();
            }
            return false;
        }
        
    }
}
