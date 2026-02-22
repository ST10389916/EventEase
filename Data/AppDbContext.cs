using EventEase.Models;

using Microsoft.EntityFrameworkCore;

namespace EventEase.Data
{
    public class AppDbContext : DbContext
        {
        public AppDbContext(DbContextOptions<DbContext> options) : base(options) { }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingDetailView> BookingDetails { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BookingDetailView>()
               .HasNoKey()
               .ToView("View_BookingsDetails");

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany()
                .HasForeignKey(b => b.EventId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
             .HasOne(e => e.EventType)
             .WithMany()
             .HasForeignKey(e => e.EventTypeId)
             .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
