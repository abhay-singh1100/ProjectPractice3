using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Interfaces
{
    using DAL.Entities;
    using Shared.DTOs;

    public interface IFlightServices
    {
        public Task AddFlight(FlightDto dto);

        public Task<Flight> GetFlightById(int id);
        public Task<List<FlightDto>> GetAllFlights();
        public Task<List<FlightDto>> SearchFlights(string source, string destination);

        public Task UpdateFlight(FlightDto dto);

        public Task DeleteFlight(Flight flight);
    }
}
