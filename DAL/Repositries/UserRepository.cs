using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositries
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository()
        {
            _context = new AppDbContext();
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                throw;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: "+ ex);
                throw;
            }
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Email==email);
        }
        public async Task<List<User>> GetAllUser()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
           
           


            try
            {
                 _context.Users.Update(user);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                throw;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " +  ex);
            }
        }
        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {

                _context.Users.Remove(user);
            }
            else
            {
                throw new Exception("User not found");
            }

            await _context.SaveChangesAsync();
        }
    }
}
