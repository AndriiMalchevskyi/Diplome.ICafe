using ICafe.Application.Interfaces;
using ICafe.Domain.Entities;
using ICafe.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICafe.Application
{
    public class UserRepository : IRepository<User>
    {
        private readonly ICafeContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ICafeContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Task<User> Add(User item)
        {
            throw new NotSupportedException();
        }

        public async Task<User> Delete(User item)
        {
            var user = await this.Get(item);
            var result = _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<User> Get(User item)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(er => er.Role)
                .Include(u => u.Photo)
                .FirstOrDefaultAsync(u => u.Id == item.Id);

            return user;
        }

        public async Task<ICollection<User>> Get(IFilter filter)
        {
            var users = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(er => er.Role)
                .Include(u => u.Photo)
                .Skip(filter.Offset)
                .Take(filter.Limit)
                .ToListAsync();
            if (!string.IsNullOrEmpty(filter.Type))
            {
                users = users.FindAll(u => {
                    foreach (var role in u.UserRoles){
                        if (role.Role.Name == filter.Type)
                        {
                            return true;
                        }
                    }
                    return false;
                });
            }

            return users;
        }

        public async Task<User> Update(User item)
        {
            var result = _context.Users.Update(item);
            await _context.SaveChangesAsync();

            return result.Entity;
        }
    }
}
