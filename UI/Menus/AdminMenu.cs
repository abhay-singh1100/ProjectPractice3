using BAL.Interfaces;
using BAL.Services;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;

public class AdminMenu
{
    private  readonly IFlightServices _flightService;
    private  readonly IBookingServices _bookingService;

    public AdminMenu(IFlightServices flightService, IBookingServices bookingService)
    {
        _flightService = flightService;
        _bookingService = bookingService;
    }

    public async Task Show()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\n=== Admin Menu ===");
                Console.WriteLine("1. Add Flight");
                Console.WriteLine("2. View Flights");
                Console.WriteLine("3. Delete Flight");
                Console.WriteLine("4. View Bookings");
                Console.WriteLine("5. Logout");

                int choice = int.Parse(Console.ReadLine() ?? "0");

                switch (choice)
                {
                    case 1: await AddFlight(); break;
                    case 2: await ViewFlights(); break;
                    case 3: await DeleteFlight(); break;
                    case 4: await ViewBookings(); break;
                    case 5: return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private async Task AddFlight()
    {
        try
        {
            var dto = new FlightDto();

            Console.Write("Flight Number: ");
            dto.FlightNumber = Console.ReadLine() ?? "";

            Console.Write("Source: ");
            dto.Source = Console.ReadLine() ?? "";

            Console.Write("Destination: ");
            dto.Destination = Console.ReadLine() ?? "";

            dto.DepartureTime = DateTime.Now.AddMinutes(5);
            dto.ArrivalTime = DateTime.Now.AddHours(2);

            Console.Write("Total Seats: ");
            dto.TotalSeats = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Price: ");
            dto.Price = decimal.Parse(Console.ReadLine() ?? "0");

            await _flightService.AddFlight(dto);
            Console.WriteLine("Flight Added Successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding flight: {ex.Message}");
        }
    }

    public   async Task ViewFlights()
    {
        try
        {
            var flights = await  _flightService.GetAllFlights();

            if (flights == null || flights.Count == 0)
            {
                Console.WriteLine("No flights available.");
                return;
            }

            var table = new ConsoleTable("ID", "Source", "Destination", "SeatsAvailable","DepartureTime","ArrivalTime","Empty Seats");

            foreach (var f in flights)
            {
                table.AddRow(f.Id, f.Source, f.Destination, f.SeatsAvailable,f.DepartureTime,f.ArrivalTime,f.TotalSeats-f.SeatsAvailable);
            }
            table.Write();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving flights: {ex.Message}");
        }
    }

    private async Task DeleteFlight()
    {
        try
        {
            Console.Write("Enter Flight ID: ");
            int id = int.Parse(Console.ReadLine() ?? "0");
            var flight = await _flightService.GetFlightById(id);

            await _flightService.DeleteFlight(flight);
            Console.WriteLine("Flight Deleted Successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting flight: {ex.Message}");
        }
    }

    private async Task ViewBookings()
    {
        try
        {
            var bookings = await _bookingService.GetAllBookings();

            if (bookings == null || bookings.Count == 0)
            {
                Console.WriteLine("No bookings available.");
                return;
            }

            var table = new ConsoleTable("BookingID", "Flight", "Seats");

            foreach (var b in bookings)
            {
                table.AddRow(b.Id, b.FlightId, b.Seats);
            }
            table.Write();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving bookings: {ex.Message}");
        }
    }
}