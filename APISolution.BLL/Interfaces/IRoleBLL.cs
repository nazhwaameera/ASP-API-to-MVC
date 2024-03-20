using APISolution.BLL.DTOs;
using System.Collections.Generic;

namespace APISolution.BLL.Interfaces
{
    public interface IRoleBLL
    {
        IEnumerable<RoleDTO> GetAllRoles();
        void AddRole(string roleName);
        void AddUserToRole(string username, int roleId);
    }
}
