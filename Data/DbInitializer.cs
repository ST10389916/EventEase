using EventEase.Data;
using EventEase.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEaseWebApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.Migrate();

            if (context.Venues.Any() || context.Events.Any() || context.Bookings.Any())
                return;

            // ============================
            // Seed Event Types
            // ============================
            var eventTypes = new List<EventType>
            {
                new EventType { Name = "Tech Conference" },
                new EventType { Name = "Business Workshop" },
                new EventType { Name = "Music Festival" },
                new EventType { Name = "Private Celebration" },
                new EventType { Name = "Community Expo" },
                new EventType { Name = "Sports Event" } // NEW
            };

            context.EventTypes.AddRange(eventTypes);
            context.SaveChanges();

            // ============================
            // Seed Venues
            // ============================
            var venues = new List<Venue>
            {
                new Venue
                {
                    VenueName = "Sandton Convention Centre",
                    Location = "161 Maude Street, Sandton",
                    Capacity = 1500,
                    IsAvailable = true,
                    ImageUrl = "/images/sandton.jpg"
                },
                new Venue
                {
                    VenueName = "Pretoria Botanical Gardens",
                    Location = "2 Cussonia Ave, Pretoria",
                    Capacity = 500,
                    IsAvailable = true,
                    ImageUrl = "/images/botanical.jpg"
                },
                new Venue
                {
                    VenueName = "Cape Town Grand Arena",
                    Location = "GrandWest, Cape Town",
                    Capacity = 3000,
                    IsAvailable = false,
                    ImageUrl = "/images/grandarena.jpg"
                },
                new Venue
                {
                    VenueName = "FNB Stadium",
                    Location = "Soccer City Ave, Nasrec, Johannesburg",
                    Capacity = 94736,
                    IsAvailable = true,
                    ImageUrl = "/images/fnbstadium.jpg"
                }
            };

            context.Venues.AddRange(venues);
            context.SaveChanges();

            // ============================
            // Seed Events
            // ============================
            var events = new List<Event>
            {
                new Event
                {
                    EventName = "AI & Cloud Summit 2026",
                    Description = "Exploring Artificial Intelligence and Cloud Innovations.",
                    EventDate = DateTime.Today.AddDays(20),
                    EventTypeId = eventTypes.First(e => e.Name == "Tech Conference").EventTypeId
                },
                new Event
                {
                    EventName = "Startup Growth Masterclass",
                    Description = "Practical strategies to scale your startup.",
                    EventDate = DateTime.Today.AddDays(35),
                    EventTypeId = eventTypes.First(e => e.Name == "Business Workshop").EventTypeId
                },
                new Event
                {
                    EventName = "Summer Music Explosion",
                    Description = "Live performances from top local artists.",
                    EventDate = DateTime.Today.AddDays(45),
                    EventTypeId = eventTypes.First(e => e.Name == "Music Festival").EventTypeId
                },
                new Event
                {
                    EventName = "National Football Championship Final",
                    Description = "The biggest football showdown of the year at FNB Stadium.",
                    EventDate = DateTime.Today.AddDays(60),
                    EventTypeId = eventTypes.First(e => e.Name == "Sports Event").EventTypeId
                }
            };

            context.Events.AddRange(events);
            context.SaveChanges();

            // ============================
            // Seed Bookings
            // ============================
            var bookings = new List<Booking>
            {
                new Booking
                {
                    BookingDate = DateTime.Today,
                    EventDate = events[0].EventDate,
                    VenueId = venues[0].VenueId,
                    EventId = events[0].EventId
                },
                new Booking
                {
                    BookingDate = DateTime.Today.AddDays(-2),
                    EventDate = events[1].EventDate,
                    VenueId = venues[1].VenueId,
                    EventId = events[1].EventId
                },
                new Booking
                {
                    BookingDate = DateTime.Today.AddDays(-7),
                    EventDate = events[2].EventDate,
                    VenueId = venues[2].VenueId,
                    EventId = events[2].EventId
                },
                new Booking
                {
                    BookingDate = DateTime.Today.AddDays(-3),
                    EventDate = events[3].EventDate,
                    VenueId = venues[3].VenueId,
                    EventId = events[3].EventId
                }
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}