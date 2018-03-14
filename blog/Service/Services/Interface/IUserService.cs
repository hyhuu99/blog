using Database;
using DTO;

namespace Services.Interface
{
    public interface IUserService
    {
        bool IsLoginSuccessful(UserDTO user);
        int? Register(UserDTO user);
        UserDTO GetUserDTO(string email);
        bool IsUpdateSuccessful(UserDTO userDTO);
        bool IsEmailExists(string email);
        User GetUser(string email);       
    }
}
