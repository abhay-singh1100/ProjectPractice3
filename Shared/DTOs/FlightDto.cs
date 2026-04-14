using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs
{
        public class FlightDto
        {
            public int Id { get; set; }

            public string FlightNumber { get; set; }

            public string Source { get; set; }

            public string Destination { get; set; }

            public DateTime DepartureTime { get; set; }

            public DateTime ArrivalTime { get; set; }

            public int TotalSeats { get; set; }

            public int SeatsAvailable { get; set; }

            public decimal Price { get; set; }
        }
    }

