using System;
using System.Collections.Generic;
using System.Text;
using Shared.DTOs;
using Shared.Enums;

namespace BAL.Validation
{
    public class BookingValidation
    {
        /// <summary>
        /// Validates booking data
        /// </summary>
        public static bool ValidateBooking(BookingDto booking, int availableSeats, out List<string> errors)
        {
            errors = new List<string>();

            // Check if booking is null
            if (booking == null)
            {
                errors.Add("Booking cannot be null");
                return false;
            }

            // Validate UserId
            if (booking.UserId <= 0)
            {
                errors.Add("Valid user ID is required");
            }

            // Validate FlightId
            if (booking.FlightId <= 0)
            {
                errors.Add("Valid flight ID is required");
            }

            // Validate number of seats
            if (booking.Seats <= 0)
            {
                errors.Add("Number of seats must be at least 1");
            }

            if (booking.Seats > 10)
            {
                errors.Add("Cannot book more than 10 seats at once");
            }

            // Validate seats don't exceed available seats
            if (booking.Seats > availableSeats)
            {
                errors.Add($"Only {availableSeats} seats available. Cannot book {booking.Seats} seats");
            }

            // Validate booking date
            if (booking.BookingDate == default(DateTime))
            {
                errors.Add("Booking date is required");
            }
            else if (booking.BookingDate < DateTime.Now.AddHours(-1)) // Allow 1 hour past for timezone issues
            {
                errors.Add("Booking date cannot be in the past");
            }

            // Validate booking status
            if (!IsValidBookingStatus(booking.Status))
            {
                errors.Add("Invalid booking status");
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Validates new booking creation
        /// </summary>
        public static bool ValidateNewBooking(BookingDto booking, int availableSeats, out List<string> errors)
        {
            errors = new List<string>();

            if (booking == null)
            {
                errors.Add("Booking cannot be null");
                return false;
            }

            // For new bookings, status should be Confirmed
            if (booking.Status != BookingStatus.Confirmed)
            {
                errors.Add("New booking status must be Confirmed");
            }

            // Use main validation
            return ValidateBooking(booking, availableSeats, out errors);
        }

        /// <summary>
        /// Validates booking cancellation
        /// </summary>
        public static bool ValidateBookingCancellation(BookingDto booking, out List<string> errors)
        {
            errors = new List<string>();

            if (booking == null)
            {
                errors.Add("Booking cannot be null");
                return false;
            }

            // Can only cancel Confirmed or Waitlisted bookings
            if (booking.Status != BookingStatus.Confirmed && booking.Status != BookingStatus.Waitlisted)
            {
                errors.Add($"Cannot cancel booking with status {booking.Status}");
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Checks if the booking status is valid
        /// </summary>
        private static bool IsValidBookingStatus(BookingStatus status)
        {
            return Enum.IsDefined(typeof(BookingStatus), status);
        }

        /// <summary>
        /// Gets list of valid booking statuses
        /// </summary>
        public static List<string> GetValidBookingStatuses()
        {
            var statuses = new List<string>();
            foreach (BookingStatus status in Enum.GetValues(typeof(BookingStatus)))
            {
                statuses.Add(status.ToString());
            }
            return statuses;
        }
    }
}

