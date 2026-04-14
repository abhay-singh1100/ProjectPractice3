using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IBookingRepository
    {
        Task AddBookingAsync(Booking booking);
        Task DeleteBooking(Booking booking);
        Task<List<Booking>> GetByUserId(int userid);
        Task<Booking> GetBookingById(int id);
        Task<List<Booking>> GetAllBookings();
        Task Update(Booking booking);
    }
}



 