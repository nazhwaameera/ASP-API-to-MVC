using APISolution.Domain;

namespace APISolution.Data.Interfaces
{
    public interface IRoleData : ICrud<Role>
    {
        Task<Task> AddUserToRole(string username, int roleId);
    }
}
