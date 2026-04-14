using System;
using System.Collections.Generic;
using System.Text;
using Shared.DTOs;

namespace BAL.Validation
{
    public class FlightValidation
    {
        /// <summary>
        /// Validates flight data
        /// </summary>
        public static bool ValidateFlight(FlightDto flight, out List<string> errors)
        {
            errors = new List<string>();

            // Check if flight is null
            if (flight == null)
            {
                errors.Add("Flight cannot be null");
                return false;
            }

            // Validate Flight Number
            if (string.IsNullOrWhiteSpace(flight.FlightNumber))
            {
                errors.Add("Flight number is required");
            }
            else if (flight.FlightNumber.Length > 10)
            {
                errors.Add("Flight number cannot exceed 10 characters");
            }

            // Validate Source
            if (string.IsNullOrWhiteSpace(flight.Source))
            {
                errors.Add("Source city is required");
            }
            else if (flight.Source.Length > 50)
            {
                errors.Add("Source city cannot exceed 50 characters");
            }

            // Validate Destination
            if (string.IsNullOrWhiteSpace(flight.Destination))
            {
                errors.Add("Destination city is required");
            }
            else if (flight.Destination.Length > 50)
            {
                errors.Add("Destination city cannot exceed 50 characters");
            }

            // Validate Source and Destination are different
            if (!string.IsNullOrWhiteSpace(flight.Source) && !string.IsNullOrWhiteSpace(flight.Destination))
            {
                if (flight.Source.Equals(flight.Destination, StringComparison.OrdinalIgnoreCase))
                {
                    errors.Add("Source and destination cities cannot be the same");
                }
            }

            // Validate Departure and Arrival times
            if (flight.DepartureTime == default(DateTime))
            {
                errors.Add("Departure time is required");
            }

            if (flight.ArrivalTime == default(DateTime))
            {
                errors.Add("Arrival time is required");
            }

            // Validate departure is before arrival
            if (flight.DepartureTime != default(DateTime) && flight.ArrivalTime != default(DateTime))
            {
                if (flight.DepartureTime >= flight.ArrivalTime)
                {
                    errors.Add("Departure time must be before arrival time");
                }

                // Validate departure is in future
                if (flight.DepartureTime < DateTime.Now)
                {
                    errors.Add("Departure time cannot be in the past");
                }
            }

            // Validate Total Seats
            if (flight.TotalSeats <= 0)
            {
                errors.Add("Total seats must be greater than 0");
            }

            // Validate Seats Available
            if (flight.SeatsAvailable < 0)
            {
                errors.Add("Available seats cannot be negative");
            }

            if (flight.SeatsAvailable > flight.TotalSeats)
            {
                errors.Add("Available seats cannot exceed total seats");
            }

            // Validate Price
            if (flight.Price <= 0)
            {
                errors.Add("Price must be greater than 0");
            }

            return errors.Count == 0;
        }

        /// <summary>
        /// Validates flight update data
        /// </summary>
        public static bool ValidateFlightUpdate(FlightDto flight, out List<string> errors)
        {
            // For updates, we can be more lenient with certain fields
            errors = new List<string>();

            if (flight == null)
            {
                errors.Add("Flight cannot be null");
                return false;
            }

            // Only validate fields that are being changed
            if (!string.IsNullOrWhiteSpace(flight.FlightNumber) && flight.FlightNumber.Length > 10)
            {
                errors.Add("Flight number cannot exceed 10 characters");
            }

            if (flight.DepartureTime != default(DateTime) && flight.ArrivalTime != default(DateTime))
            {
                if (flight.DepartureTime >= flight.ArrivalTime)
                {
                    errors.Add("Departure time must be before arrival time");
                }
            }

            if (flight.TotalSeats > 0 && flight.SeatsAvailable > flight.TotalSeats)
            {
                errors.Add("Available seats cannot exceed total seats");
            }

            if (flight.Price < 0)
            {
                errors.Add("Price cannot be negative");
            }

            return errors.Count == 0;
        }
    }
}

