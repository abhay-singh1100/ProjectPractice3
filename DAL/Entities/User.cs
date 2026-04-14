using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.Enums;

namespace DAL.Entities
{
    public class User
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int  Id { get; set; }
        public string? Password {  get; set; }
        public UserRole Role { get; set; }
        public List<Booking>? Bookings { get; set; }
        
    }
}

