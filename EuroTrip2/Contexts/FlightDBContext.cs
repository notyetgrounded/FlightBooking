using EuroTrip2.Models;
using Microsoft.EntityFrameworkCore;

namespace EuroTrip2.Contexts
{
    public class FlightDBContext : DbContext
    {
        public FlightDBContext(DbContextOptions<FlightDBContext> options) : base(options)
        {

        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripRoute> TripRoutes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<SeatStatus> SeatStatuses { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>().HasOne(x => x.Trip).WithMany(x => x.Bookings);
            modelBuilder.Entity<Booking>().HasOne(x => x.User).WithMany(x => x.Bookings);
            modelBuilder.Entity<Booking>().HasOne(x => x.FromBooking).WithOne(x => x.NextBooking);
            modelBuilder.Entity<Seat>().HasOne(x => x.Flight).WithMany(x => x.Seats).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Trip>().HasOne(x => x.TripRoute).WithMany(x => x.Trips);
            modelBuilder.Entity<Trip>().HasOne(x => x.Flight).WithMany(x => x.Trips);
            modelBuilder.Entity<TripRoute>().HasOne(x => x.Source).WithMany(x => x.Sources).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<TripRoute>().HasOne(x => x.Destination).WithMany(x => x.Destinations).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SeatStatus>().HasKey(x => new { x.Seat_Id, x.Trip_Id });
            modelBuilder.Entity<SeatStatus>().HasOne(x => x.Trip).WithMany(x => x.SeatStatuses).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<SeatStatus>().HasOne(x => x.Seat).WithMany(x => x.SeatStatuses).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
