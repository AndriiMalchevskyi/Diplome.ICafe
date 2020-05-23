using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ICafe.Application.Models.User
{
    public class UserPasswordToResetDto
    {
        [Required]
        public string oldPassword { get; set; }
        
        [Required]
        public string newPassword { get; set; }
    }
}
