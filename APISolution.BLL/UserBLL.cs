using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APISolution.BLL.DTOs;
using APISolution.BLL.Interfaces;
using APISolution.Data.Interfaces;
using APISolution.Domain;
using AutoMapper;

namespace APISolution.BLL
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserData _userData;
        private readonly IMapper _mapper;

        public UserBLL(IUserData userData, IMapper mapper)
        {
            _userData = userData;
            _mapper = mapper;
        }

        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            var result= await _userData.ChangePassword(username, newPassword);
            return result;
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await _userData.GetAll();
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            return usersDTO;
        }

        public async Task<IEnumerable<UserDTO>> GetAllWithRoles()
        {
            var users = await _userData.GetAllWithRoles();
            var usersDTO = _mapper.Map<IEnumerable<UserDTO>>(users);
            return usersDTO;
        }

        public async Task<UserDTO> GetByUsername(string username)
        {
            var user = await _userData.GetByUsername(username);
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO> GetUserWithRoles(string username)
        {
            var user = await _userData.GetUserWithRoles(username);
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO> Insert(UserCreateDTO entity)
        {
            var user = _mapper.Map<User>(entity);
            var userDomain = await _userData.Insert(user);
            var userDTO = _mapper.Map<UserDTO>(userDomain);
            return userDTO;
        }

        public async Task<UserDTO> Login(LoginDTO loginDTO)
        {
            var user = await _userData.Login(loginDTO.Username, loginDTO.Password);
            var userDTO = _mapper.Map<UserDTO>(user);
            return userDTO;
        }

        public async Task<UserDTO> Update(UserDTO entity)
        {
            var user = _mapper.Map<User>(entity);
            var userDomain = await _userData.Update(user);
            var userDTO = _mapper.Map<UserDTO>(userDomain);
            return userDTO;
        }
    }
}
