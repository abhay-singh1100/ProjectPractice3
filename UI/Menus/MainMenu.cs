using BAL.Interfaces;
using BAL.Services;
using DAL.Entities;
using Shared.DTOs;
using Shared.Enums;
using Shared.Seeding;
using System;
using System.Collections.Generic;
using System.Text;


public class MainMenu
{
    private readonly IAuthServices _authService;
    private readonly IFlightServices _flightService;
    private readonly IBookingServices _bookingService;

    public MainMenu(IAuthServices authService, IFlightServices flightService, IBookingServices bookingService)
    {
        _authService = authService;
        _flightService = flightService;
        _bookingService = bookingService;
    }

    public async Task Show()
    {
        while (true)
        {
            try
            {
                Console.WriteLine("\n=== Flight Booking System ===");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        await Register();
                        break;

                    case 2:
                        await Login();
                        break;

                    case 3:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public async Task Register()
    {
        try
        {
            var dto = new UserDto();

            Console.Write("Name: ");
            dto.Name = Console.ReadLine();

            Console.Write("Email: ");
            dto.Email = Console.ReadLine();

            Console.Write("Password: ");
            dto.Password = Console.ReadLine();

            dto.Role = UserRole.User;

            var registeredUser = await _authService.Register(dto);

            Console.WriteLine("Registered Successfully!");
            Console.WriteLine($"Welcome {dto.Name}");

            // Send confirmation email
            bool emailSent = await EmailSender.SendEmailAsync(dto.Email);
            if (emailSent)
            {
                Console.WriteLine("Confirmation email has been sent.");
            }
            else
            {
                Console.WriteLine("Failed to send confirmation email, but you can continue.");
            }

            var loggedInUser = await _authService.Login(dto.Email, dto.Password);
            await new UserMenu(_flightService, _bookingService, loggedInUser.Id).Show();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Registration failed: {ex.Message}");
        }
    }

    public async Task Login()
    {
        try
        {
            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            var user = await _authService.Login(email, password);

            Console.WriteLine($"Welcome {user.Name}");
            bool emailSent = await EmailSender.SendEmailAsync(email);
            if (emailSent)
            {
                Console.WriteLine("Confirmation email has been sent.");
            }
            else
            {
                Console.WriteLine("Failed to send confirmation email, but you can continue.");
            }

            if (user.Role == UserRole.Admin)
                await new AdminMenu(_flightService, _bookingService).Show();
            else
                await new UserMenu(_flightService, _bookingService, user.Id).Show();

            Console.Clear();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");
        }
    }
}
