using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APISolution.BLL.DTOs
{
    public class LoginDTO
    {
        public string? Username { get; set; }

        public string? Password { get; set; }

        [JsonIgnore]
        public string? Token { get; set; }
    }
}