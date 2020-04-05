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
                //var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                //var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                    new Role{Name = "VIP"},
                };

                foreach (var role in roles)
                {
                    _roleMager.CreateAsync(role).Wait();
                }

                //foreach (var user in users)
                //{
                //    _userManager.CreateAsync(user, "password").Wait();
                //    _userManager.AddToRoleAsync(user, "Member").Wait();
                //}

                var adminUser = new User
                {
                    UserName = "Admin"
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("Admin").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
                }

            }

        }
    }
}
