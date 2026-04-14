using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace DAL.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FlightId { get; set; }
        public int SeatsBooked { get; set; }
        public DateTime BookingDate { get; set; }
        public BookingStatus Status { get; set; }

        public User? User { get; set; }
        public Flight? Flight { get; set; }

    }
}
