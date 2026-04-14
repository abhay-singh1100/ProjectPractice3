using BAL.Interfaces;
using BAL.Validation;
using DAL.Entities;
using DAL.Interfaces;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class FlightService : IFlightServices
    {
        private readonly IFlightRepository _flightRepo;

        public FlightService(IFlightRepository flightRepo)
        {
            _flightRepo = flightRepo;
        }

        public async Task AddFlight(FlightDto dto)
        {
            if (!FlightValidation.ValidateFlight(dto, out var errors))
            {
                throw new Exception(string.Join(", ", errors));
            }

            var flight = new Flight
            {
                FlightNumber = dto.FlightNumber,
                Source = dto.Source,
                Destination = dto.Destination,
                DepatureTime = dto.DepartureTime,
                ArrivalTime = dto.ArrivalTime,
                TotalSeats = dto.TotalSeats,
                SeatsAvailable = dto.TotalSeats,
                Price = dto.Price
            };

            await _flightRepo.AddFlightAsync(flight);
        }

        public async Task<List<FlightDto>> GetAllFlights()
        {
            var flights = await _flightRepo.GetAll();
            var flightDtos = flights
                .Select(f => new FlightDto
                {
                    Id = f.Id,
                    FlightNumber = f.FlightNumber ?? "",
                    Source = f.Source ?? "",
                    Destination = f.Destination ?? "",
                    DepartureTime = f.DepatureTime,
                    ArrivalTime = f.ArrivalTime,
                    SeatsAvailable = f.SeatsAvailable,
                    Price = f.Price
                }).ToList();
            return flightDtos;
        }

        public async Task<FlightDto> GetFlightById(int id)
        {
            var f = await _flightRepo.GetFlightById(id);
            if (f == null)
                throw new Exception("Flight not found");

            var flightDto = new FlightDto
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber ?? "",
                Source = f.Source ?? "",
                Destination = f.Destination ?? "",
                DepartureTime = f.DepatureTime,
                ArrivalTime = f.ArrivalTime,
                SeatsAvailable = f.SeatsAvailable,
                Price = f.Price
            };
            return flightDto;
        }

        public async Task<List<FlightDto>> SearchFlights(string source, string destination)
        {
            var flights = await _flightRepo.SearchFlight(source, destination);
            var flightDtos = flights
                .Select(f => new FlightDto
                {
                    Id = f.Id,
                    Source = f.Source ?? "",
                    Destination = f.Destination ?? "",
                    SeatsAvailable = f.SeatsAvailable,
                    Price = f.Price
                }).ToList();
            return flightDtos;
        }

        public async Task UpdateFlight(FlightDto dto)
        {
            if (!FlightValidation.ValidateFlightUpdate(dto, out var errors))
            {
                throw new Exception(string.Join(", ", errors));
            }

            var flight = await _flightRepo.GetFlightById(dto.Id);

            if (flight == null)
                throw new Exception("Flight not found");

            flight.Source = dto.Source;
            flight.Destination = dto.Destination;
            flight.Price = dto.Price;
            flight.SeatsAvailable = dto.SeatsAvailable;

            await _flightRepo.UpdateFlightAsync(flight);
        }

        public async Task DeleteFlight(int id)
        {
            await _flightRepo.DeleteFligth(id);
        }
    }
}




