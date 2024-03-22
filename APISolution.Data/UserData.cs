using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APISolution.Data.Interfaces;
using APISolution.Domain;
using Microsoft.EntityFrameworkCore;

namespace APISolution.Data
{
    public class UserData : IUserData
    {
        private readonly AppDbContext _context;
        public UserData(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Task> ChangePassword(string username, string newPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                user.Password = Helpers.Md5Hash.GetHash(newPassword);
                await _context.SaveChangesAsync();
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex.Message}");
            }
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                var users = await _context.Users.OrderBy(u => u.Username).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("There is no user data.");
            }
        }

        public async Task<IEnumerable<User>> GetAllWithRoles()
        {
            try
            {
                // Retrieve users without roles (only user data)
                var usersWithoutRoles = await _context.Users.OrderBy(u => u.Username).ToListAsync();

                // Retrieve roles for each user separately
                foreach (var user in usersWithoutRoles)
                {
                    // Load roles for the current user
                    await _context.Entry(user)
                        .Collection(u => u.Roles)
                        .LoadAsync();
                }

                return usersWithoutRoles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetByUsername(string username)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetUserWithRoles(string username)
        {
            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            return user;
        }

        public async Task<User> Insert(User entity)
        {
            try
            {
                _context.Users.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"{ex.Message}");
            }
        }

        public async Task<User> Login(string username, string password)
        {
            try
            {
                // Find the user by username and password
                var user = await _context.Users
                    .Include(u => u.Roles) // Eager loading of roles
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == Helpers.Md5Hash.GetHash(password));

                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> Update(User entity)
        {
            try
            {
                var user = await GetByUsername(entity.Username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                user.Username = entity.Username;
                user.FirstName = entity.FirstName;
                user.LastName = entity.LastName;
                user.Email = entity.Email;
                user.Address = entity.Address;
                user.Telp = entity.Telp;

                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}

