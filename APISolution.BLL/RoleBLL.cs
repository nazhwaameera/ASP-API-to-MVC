using APISolution.BLL.DTOs;
using APISolution.BLL.Interfaces;
using APISolution.Data.Interfaces;
using APISolution.Domain;
using AutoMapper;

namespace APISolution.BLL
{
	public class RoleBLL : IRoleBLL
	{
		private readonly IRoleData _roleData;
		private readonly IMapper _mapper;

		public RoleBLL(IRoleData roleData, IMapper mapper)
		{
			_roleData = roleData;
			_mapper = mapper;
		}

        public async Task<Task> AddUserToRole(string username, int roleId)
        {
            var result = await _roleData.AddUserToRole(username, roleId);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _roleData.Delete(id);
            return result;
        }

        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var roles = await _roleData.GetAll();
            var rolesDTO = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            return rolesDTO;
        }

        public async Task<RoleDTO> GetById(int id)
        {
            var role = await _roleData.GetById(id);
            var roleDTO = _mapper.Map<RoleDTO>(role); 
            return roleDTO;
        }

        public async Task<RoleDTO> Insert(RoleDTO entity)
        {
            var role = _mapper.Map<Role>(entity);
            var roleDomain = await _roleData.Insert(role);
            var roleDTO = _mapper.Map<RoleDTO>(roleDomain);
            return roleDTO;
        }

        public async Task<RoleDTO> Update(RoleDTO entity)
        {
            var role = _mapper.Map<Role>(entity);
            var roleDomain = await _roleData.Update(role);
            var roleDTO = _mapper.Map<RoleDTO>(roleDomain);
            return roleDTO;
        }
    }
}
