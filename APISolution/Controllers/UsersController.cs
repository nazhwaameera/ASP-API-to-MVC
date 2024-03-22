using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APISolution.BLL.DTOs;
using APISolution.BLL.Interfaces;
using APISolution.Helpers;
using APISolution.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace APISolution.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBLL _userBLL;
        private readonly AppSettings _appSettings;

        public UsersController(IUserBLL userBLL, IOptions<AppSettings> appSettings)
        {
            _userBLL = userBLL;
            _appSettings = appSettings.Value;
        }

        [HttpPost("/changepass")]
        public async Task<ActionResult> ChangePassword(string username, string newPassword)
        {
            var result = await _userBLL.ChangePassword(username, newPassword);
            return Ok("Password changed");
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            return await _userBLL.GetAll();
        }

        [HttpGet("/withroles")]
        public async Task<IEnumerable<UserDTO>> GetAllWithRoles()
        {
            return await _userBLL.GetAllWithRoles();
        }

        [HttpGet("/username")]
        public async Task<ActionResult<UserDTO>> GetByUsername([FromQuery]string username)
        {
            try
            {
                var user = await _userBLL.GetByUsername(username);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("/userwrole")]
        public async Task<UserDTO> GetUserWithRoles(string username)
        {
            try
            {
                return await _userBLL.GetUserWithRoles(username);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("/login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            try
            {
                var user = await _userBLL.Login(loginDTO);
                if (user == null)
                {
                    return Unauthorized();
                }
                else
                {
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, loginDTO.Username));
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var userWithToken = new UserWithToken
                    {
                        Username = loginDTO.Username,
                        Roles = user.Roles,
                        Token = tokenHandler.WriteToken(token)
                    };
                    return Ok(userWithToken);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Insert(UserCreateDTO userCreateDTO)
        {
            try
            {
                var user = await _userBLL.Insert(userCreateDTO);
                return CreatedAtAction(nameof(GetByUsername), new { username = user.Username }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<ActionResult<UserDTO>> Update(UserDTO userDTO)
        {
            try
            {
                var updatedUser = await _userBLL.Update(userDTO);
                if (updatedUser == null)
                {
                    return NotFound();
                }
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
