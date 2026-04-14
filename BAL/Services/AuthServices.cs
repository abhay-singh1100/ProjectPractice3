using BAL.Interfaces;
using BAL.Validation;
using DAL.Entities;
using DAL.Interfaces;
using Shared.DTOs;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Services
{
    public class AuthService : IAuthServices
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserDto> Register(UserDto dto)
        {
            if (!UserValidation.ValidateUserRegistration(dto, out var errors))
            {
                throw new Exception(string.Join(", ", errors));
            }

            var existing = await _userRepo.GetUserByEmailAsync(dto.Email);

            if (existing != null)
                throw new Exception("User with this email already exists");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Role = dto.Role
            };

            await _userRepo.AddUserAsync(user);

            dto.Id = user.Id;
            return dto;
        }

        public async Task<UserDto> Login(string email, string password)
        {
            //if (!UserValidation.ValidateLogin(email, password, out var errors))
            //{
            //    throw new Exception(string.Join(", ", errors));
            //}

            var user = await _userRepo.GetUserByEmailAsync(email);

            if (user == null || user.Password != password)
                throw new Exception("Invalid email or password");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name ?? "",
                Email = user.Email ?? "",
                Role = user.Role
            };
        }
    }
}

