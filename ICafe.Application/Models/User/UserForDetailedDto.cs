using ICafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.User
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<UserRole> Roles { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}
