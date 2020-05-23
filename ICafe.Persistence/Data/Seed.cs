using ICafe.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICafe.Persistence.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleMager;


        public Seed(UserManager<User> userManager, RoleManager<Role> roleMager)
        {
            _userManager = userManager;
            _roleMager = roleMager;
        }

        public void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                var roles = new List<Role>
                {
                    new Role{Name = "visitor"},
                    new Role{Name = "worker"},
                    new Role{Name = "waiter"},
                    new Role{Name = "cook"},
                    new Role{Name = "admin"},
                    new Role{Name = "sysadmin"},
                    new Role{Name = "root"},
                };

                foreach (var role in roles)
                {
                    _roleMager.CreateAsync(role).Wait();
                }

                var adminUser = new User
                {
                    UserName = "Admin",
                    Email = "asmalchevskyi@gmail.com"
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "root" }).Wait();
                }

            }

        }
    }
}
