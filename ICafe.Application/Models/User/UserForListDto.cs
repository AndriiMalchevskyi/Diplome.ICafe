using ICafe.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Application.Models.User
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}
