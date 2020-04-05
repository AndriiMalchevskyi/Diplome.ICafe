using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
