using System.ComponentModel.DataAnnotations;

namespace APISolution.BLL.DTOs
{
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}