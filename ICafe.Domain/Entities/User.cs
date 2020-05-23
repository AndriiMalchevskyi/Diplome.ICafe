using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICafe.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public DiscountCard Discount { get; set; }
        public ICollection<Favorites> FavoriteCollection { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }

        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}
