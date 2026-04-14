using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FlightId { get; set; }
        public int Seats { get; set; }
        public DateTime BookingDate { get; set; }

        public BookingStatus Status{ get; set; }
    }
}
