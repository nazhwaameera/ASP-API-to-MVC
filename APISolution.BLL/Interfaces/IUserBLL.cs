using APISolution.BLL.DTOs;
using System.Collections.Generic;

namespace APISolution.BLL.Interfaces
{
    public interface IUserBLL
    {
        Task<Task> ChangePassword(string username, string newPassword);
        Task<IEnumerable<UserDTO>> GetAll();
        Task<UserDTO> GetByUsername(string username);
        Task<UserDTO> Insert(UserCreateDTO entity);
        Task<UserDTO> Update(UserDTO entity);
        Task<UserDTO> Login(LoginDTO loginDTO);
        Task<UserDTO> GetUserWithRoles(string username);
        Task<IEnumerable<UserDTO>> GetAllWithRoles();
    }
}