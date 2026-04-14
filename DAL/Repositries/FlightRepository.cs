using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repositries
{
    public class FlightRepository:IFlightRepository
    {
        private readonly AppDbContext _context;
        public FlightRepository()
        {
            _context = new AppDbContext();
        }

        public async Task AddFlightAsync(Flight flight)
        {
            try
            {
                await _context.Flights.AddAsync(flight);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: "+ ex);
                throw;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                throw;
            }

        }
        public async Task DeleteFligth(int id )
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight != null)
            {
                 _context.Remove(flight);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Flight not found");
            }
        }

        public async Task<Flight> GetFlightById(int id)
        {
            return await _context.Flights.FirstOrDefaultAsync(f => f.Id == id);
            
           
        }

        public async Task UpdateFlightAsync(Flight flight)
        {
            try
            {
                _context.Flights.Update(flight);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
            try
            {
                await _context.SaveChangesAsync();

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: "+ ex);
            }
        }

        public async Task<List<Flight>> GetAll()
        {
            return await _context.Flights.ToListAsync();
        }

        public async Task<List<Flight>> SearchFlight(string source, string destination)
        {
            return await _context.Flights
                .Where(f => f.Source == source && f.Destination == destination)
                .ToListAsync();
        }
    }
}
