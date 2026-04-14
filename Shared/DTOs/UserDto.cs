using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;


namespace Shared.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; } // For input (hash in service)

        public UserRole Role { get; set; }
    }
}
