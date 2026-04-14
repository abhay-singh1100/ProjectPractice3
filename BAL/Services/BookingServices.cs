using BAL.Interfaces;
using BAL.Validation;
using DAL.Interfaces;
using DAL.Entities;
using Shared.DTOs;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class BookingService : IBookingServices
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IFlightRepository _flightRepo;

        public BookingService(
            IBookingRepository bookingRepo,
            IFlightRepository flightRepo)
        {
            _bookingRepo = bookingRepo;
            _flightRepo = flightRepo;
        }

        public async Task BookFlight(BookingDto dto)
        {
            var flight = await _flightRepo.GetFlightById(dto.FlightId);
            int availableSeats = flight?.SeatsAvailable ?? 0;

            if (!BookingValidation.ValidateNewBooking(dto, availableSeats, out var errors))
            {
                throw new Exception(string.Join(", ", errors));
            }

            if (flight == null)
                throw new Exception("Flight not found");

            if (flight.SeatsAvailable < dto.Seats)
                throw new Exception($"Not enough seats available. Only {flight.SeatsAvailable} seats left");

            // Deduct seats
            flight.SeatsAvailable -= dto.Seats;
            await _flightRepo.UpdateFlightAsync(flight);

            var booking = new Booking
            {
                UserId = dto.UserId,
                FlightId = dto.FlightId,
                SeatsBooked = dto.Seats,
                BookingDate = DateTime.Now,
                Status = BookingStatus.Confirmed
            };

            await _bookingRepo.AddBookingAsync(booking);
        }

        public async Task CancelBooking(int bookingId)
        {
            var booking = await _bookingRepo.GetBookingById(bookingId);

            if (booking == null)
                throw new Exception("Booking not found");

            var bookingDto = new BookingDto
            {
                Id = booking.Id,
                UserId = booking.UserId,
                FlightId = booking.FlightId,
                Seats = booking.SeatsBooked,
                BookingDate = booking.BookingDate,
                Status = booking.Status
            };

            if (!BookingValidation.ValidateBookingCancellation(bookingDto, out var errors))
            {
                throw new Exception(string.Join(", ", errors));
            }

            var flight = await _flightRepo.GetFlightById(booking.FlightId);

            if (flight != null)
            {
                flight.SeatsAvailable += booking.SeatsBooked;
                await _flightRepo.UpdateFlightAsync(flight);
            }

            booking.Status = BookingStatus.Cancelled;
            await _bookingRepo.Update(booking);
        }

        public async Task<List<BookingDto>> GetUserBookings(int userId)
        {
            var bookings = await _bookingRepo.GetByUserId(userId);
            var bookingDtos = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                FlightId = b.FlightId,
                Seats = b.SeatsBooked,
                BookingDate = b.BookingDate,
                Status = b.Status
            }).ToList();
            return bookingDtos;
        }

        public async Task<List<BookingDto>> GetAllBookings()
        {
            var bookings = await _bookingRepo.GetAllBookings();
            var bookingDtos = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                FlightId = b.FlightId,
                Seats = b.SeatsBooked,
                BookingDate = b.BookingDate,
                Status = b.Status
            }).ToList();
            return bookingDtos;
        }
    }
}