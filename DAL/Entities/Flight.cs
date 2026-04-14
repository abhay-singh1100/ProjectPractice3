using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public  string? FlightNumber { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        public DateTime DepatureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int TotalSeats { get; set; }
        public int SeatsAvailable { get; set; }
        public decimal Price { get; set; }
        public List<Booking> Bookings { get; set; }
    }
}
