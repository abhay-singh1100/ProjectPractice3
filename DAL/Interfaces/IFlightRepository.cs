using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IFlightRepository
    {
        Task AddFlightAsync(Flight flight);
        Task DeleteFligth(int id);

        Task<Flight> GetFlightById(int id);

        Task UpdateFlightAsync(Flight flight);

        Task<List<Flight>> GetAll();

        Task<List<Flight>> SearchFlight(string source, string destination);

    }
}


