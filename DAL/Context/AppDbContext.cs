
using DAL.Entities;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Shared.Constants;
using Shared.Enums;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DAL.Context
{
    public class AppDbContext: DbContext
    {
        public AppDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."))
                    .AddJsonFile("AppSetting.json", optional: false)
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Flight> Flights { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u=>u.Id); 
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
                
                entity.Property(u => u.Role).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();

            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.UserId).IsRequired();
                entity.Property(b => b.BookingDate).IsRequired();
                entity.Property(b=>b.SeatsBooked).IsRequired();
                entity.Property(b => b.FlightId).IsRequired();
                entity.Property(b => b.Status).IsRequired();
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.FlightNumber).IsRequired().HasMaxLength(100);
                entity.Property(f => f.Source).IsRequired().HasMaxLength(100);
                entity.Property(f => f.Destination).IsRequired().HasMaxLength(100);
                entity.Property(f => f.DepatureTime).IsRequired();
                entity.Property(f => f.ArrivalTime).IsRequired();
                entity.Property(f => f.TotalSeats).IsRequired();
                entity.Property(f=>f.SeatsAvailable).IsRequired();
                entity.Property(f=>f.Price).IsRequired().HasColumnType("decimal(10,2)");
            });

            // ===========Relations=========== \\

            //user (1) -> (Many) Bookings 
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 10,
                    Name = "patient1",
                    Email = "patient1@gmail.com",
                    Password = "123",
                    Role = UserRole.Admin,
                    //dotnet ef migrations add AddUserSeedData
                }
                );

        }
    }
}


         