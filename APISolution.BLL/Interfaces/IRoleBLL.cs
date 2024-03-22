using APISolution.BLL.DTOs;
using System.Collections.Generic;

namespace APISolution.BLL.Interfaces
{
    public interface IRoleBLL
    {
        Task<IEnumerable<RoleDTO>> GetAll();
        Task<RoleDTO> GetById(int id);
        Task<RoleDTO> Insert(RoleDTO entity);
        Task<RoleDTO> Update(RoleDTO entity);
        Task<bool> Delete(int id);
        Task<Task> AddUserToRole(string username, int roleId);
    }
}
