using BAL.Interfaces;
using BAL.Interfaces
    ;
using BAL.Services;
using DAL.Context;
using DAL.Interfaces;
using DAL.Repositries;
using System;
using System.Collections.Generic;
using System.Text;
//using UI.Menus;

class Program
{
    static async Task Main(string[] args)
    {
        var context = new AppDbContext();

        // Repositories
        IUserRepository userRepo = new UserRepository();
        IFlightRepository flightRepo = new FlightRepository();
        IBookingRepository bookingRepo = new BookingRepository();

        // Services
        IAuthServices authService = new AuthService(userRepo);
        IFlightServices flightService = new FlightService(flightRepo);
        IBookingServices bookingService = new BookingService(bookingRepo, flightRepo);

        // Start App
        var menu = new MainMenu(authService, flightService, bookingService);
        await menu.Show();
    }
}