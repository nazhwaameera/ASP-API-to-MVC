using APISolution.BLL.DTOs;

namespace APISolution.ViewModels
{
    public class UserWithToken
    {
        public string Username { get; set; }
        public IEnumerable<RoleDTO> Roles { get; set; }
        public string? Token { get; set; }
    }
}
