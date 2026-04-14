using System;
using System.Collections.Generic;
using System.Text;
using DAL.Entities;
using Shared.DTOs;

namespace BAL.Interfaces
{

    public interface IBookingServices
    {
        // BOOK
        public Task BookFlight(BookingDto dto);

        // CANCEL
        public Task CancelBooking(int id);

        // USER BOOKINGS
        Task<List<BookingDto>> GetUserBookings(int userId);

        // ADMIN VIEW
        Task<List<BookingDto>> GetAllBookings();
    }

}
