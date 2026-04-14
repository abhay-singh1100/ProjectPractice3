using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositries
{
    public class BookingRepository:IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository()
        {
            _context = new AppDbContext();
        }
        public async Task AddBookingAsync(Booking booking)
        {
            try
            {
                await _context.Bookings.AddAsync(booking);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: "+ ex);
                throw;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: "+ex);
                throw;
            }
        }
        public async Task DeleteBooking(Booking booking)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Booking>> GetByUserId(int userid)
        {
            var bookings = await _context.Bookings.Where(b => b.UserId == userid).ToListAsync();
            if (bookings == null || bookings.Count == 0)
            {
                throw new Exception("No bookings founded!");
            }
            return bookings;
        }
        public async Task<Booking> GetBookingById(int id)
        {
            return await _context.Bookings.FindAsync(id);
        }
        public async Task<List<Booking>> GetAllBookings()
        {
            return await _context.Bookings.ToListAsync();
        }
        public async Task Update(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
