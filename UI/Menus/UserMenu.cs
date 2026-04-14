using BAL.Interfaces;
using BAL.Services;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class UserMenu
{
    private readonly IFlightServices _flightService;
    private readonly IBookingServices _bookingService;
    private readonly int _userId;

    public UserMenu(IFlightServices flightService, IBookingServices bookingService, int userId)
    { 
        _flightService = flightService;
        _bookingService = bookingService;
        _userId = userId;
    }

    public async Task Show()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\n=== User Menu ===");
                Console.WriteLine("1. Search Flights");
                Console.WriteLine("2. Book Flight");
                Console.WriteLine("3. My Bookings");
                Console.WriteLine("4. Cancel Booking");
                Console.WriteLine("5. Logout");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1: await SearchFlights(); break;
                    case 2: await BookFlight(); break;
                    case 3: await ViewBookings(); break;
                    case 4: await CancelBooking(); break;
                    case 5: return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private async Task SearchFlights()
    {
        try
        {
            Console.Write("Source: ");
            string src = Console.ReadLine();

            Console.Write("Destination: ");
            string dest = Console.ReadLine();

            var flights = await _flightService.SearchFlights(src, dest);

            if (flights == null || flights.Count == 0)
            {
                Console.WriteLine("No flights found for the given route.");
                return;
            }

            var table = new ConsoleTable("ID", "Source", "Destination", "SeatsAvailable", "Price", "DepartureTime", "ArrivalTime");

            foreach (var f in flights)
            {
                table.AddRow(f.Id, f.Source, f.Destination, f.SeatsAvailable, f.Price,f.DepartureTime,f.ArrivalTime);
            }
            table.Write();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error searching flights: {ex.Message}");
        }
    }

    
    private async Task BookFlight()
    {
        var flights = await _flightService.GetAllFlights();

        if (flights == null || flights.Count == 0)
        {
            Console.WriteLine("No flights available.");
            return;
        }

        var table = new ConsoleTable("ID", "Source", "Destination", "SeatsAvailable", "DepartureTime", "ArrivalTime", "Empty Seats");

        foreach (var f in flights)
        {
            table.AddRow(f.Id, f.Source, f.Destination, f.SeatsAvailable, f.DepartureTime, f.ArrivalTime, f.TotalSeats - f.SeatsAvailable);
        }
        table.Write();


        try
        {
            
            Console.Write("Flight ID: ");
            int id = int.Parse(Console.ReadLine());

            Console.Write("Seats: ");
            int seats = int.Parse(Console.ReadLine());

           

            await _bookingService.BookFlight(new BookingDto
            {
                UserId = _userId,
                FlightId = id,
                Seats = seats,
                BookingDate = DateTime.Now,
                Status = Shared.Enums.BookingStatus.Confirmed
            });

            Console.WriteLine("Booking Successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Booking failed: {ex.Message}");
        }
    }

    private async Task ViewBookings()
    {
        try
        {
            
            var bookings = await _bookingService.GetUserBookings(_userId);

            if (bookings == null || bookings.Count == 0)
            {
                Console.WriteLine("You have no bookings yet.");
                return;
            }

            var table = new ConsoleTable("ID", "Flight", "Seats", "Status","BookingDate");

            foreach (var b in bookings)
            {
                table.AddRow(b.Id, b.FlightId, b.Seats, b.Status,b.BookingDate);
            }
            table.Write();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving bookings: {ex.Message}");
        }
    }

    private async Task CancelBooking()
    {
        try
        {
            await ViewBookings();
            Console.Write("Booking ID: ");
            int id = int.Parse(Console.ReadLine());

            var bookings = await _bookingService.GetUserBookings(_userId);
            if(bookings.FindAll(x => x.Id == id) != null)
            {
                await _bookingService.CancelBooking(id);
                Console.WriteLine("Booking Cancelled Successfully!");

            }


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cancelling booking: {ex.Message}");
        }
    }
}
