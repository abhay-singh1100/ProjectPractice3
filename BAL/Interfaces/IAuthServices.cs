using System;
using System.Collections.Generic;
using System.Text;

using Shared.DTOs;

namespace BAL.Interfaces
{
    public interface IAuthServices
    {
        public Task<UserDto> Register(UserDto userDto);
        public Task<UserDto> Login(string email, string password);
    }
}
