using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
       
        Task AddUserAsync(User user);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUser();
        Task UpdateUserAsync(User user);
        Task DeleteUser(int id);
    }
}


