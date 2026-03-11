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
            var tech = new EventType { Name = "Tech Conference" };
            var business = new EventType { Name = "Business Workshop" };
            var music = new EventType { Name = "Music Festival" };
            var privateCelebration = new EventType { Name = "Private Celebration" };
            var expo = new EventType { Name = "Community Expo" };
            var sports = new EventType { Name = "Sports Event" };

            context.EventTypes.AddRange(tech, business, music, privateCelebration, expo, sports);
            context.SaveChanges();

            // ============================
            // Seed Venues
            // ============================
            var sandton = new Venue
            {
                VenueName = "Sandton Convention Centre",
                Location = "161 Maude Street, Sandton",
                Capacity = 1500,
                IsAvailable = true,
                ImageUrl = "/images/Sandton Convention Centre.jpg"
            };

            var pretoria = new Venue
            {
                VenueName = "Pretoria Botanical Gardens",
                Location = "2 Cussonia Ave, Pretoria",
                Capacity = 500,
                IsAvailable = true,
                ImageUrl = "/images/Pretoria Botanical Gardens.jpg"
            };

            var capeTown = new Venue
            {
                VenueName = "Cape Town Grand Arena",
                Location = "GrandWest, Cape Town",
                Capacity = 3000,
                IsAvailable = false,
                ImageUrl = "/images/Cape Town Grand Arena.jpg"
            };

            var fnb = new Venue
            {
                VenueName = "FNB Stadium",
                Location = "Soccer City Ave, Johannesburg",
                Capacity = 94736,
                IsAvailable = true,
                ImageUrl = "/images/FNB Stadium.jpg"
            };

            context.Venues.AddRange(sandton, pretoria, capeTown, fnb);
            context.SaveChanges();

            // ============================
            // Seed Events
            // ============================


            var aiSummit = new Event
            {
                EventName = "AI & Cloud Summit 2026",
                Description = "Exploring AI and Cloud innovations.",
                EventDate = DateTime.Today.AddDays(20),
                EventTypeId = tech.EventTypeId,
                VenueId = sandton.VenueId // ✅ FIX
            };

            var startup = new Event
            {
                EventName = "Startup Growth Masterclass",
                Description = "Scale your startup effectively.",
                EventDate = DateTime.Today.AddDays(35),
                EventTypeId = business.EventTypeId,
                VenueId = pretoria.VenueId // ✅ FIX
            };

            var summerMusic = new Event
            {
                EventName = "Summer Music Explosion",
                Description = "Top local artists performing live.",
                EventDate = DateTime.Today.AddDays(45),
                EventTypeId = music.EventTypeId,
                VenueId = capeTown.VenueId // ✅ FIX
            };

            var football = new Event
            {
                EventName = "National Football Championship Final",
                Description = "The biggest football showdown of the year.",
                EventDate = DateTime.Today.AddDays(60),
                EventTypeId = sports.EventTypeId,
                VenueId = fnb.VenueId // ✅ FIX
            };

            context.Events.AddRange(aiSummit, startup, summerMusic, football);
            context.SaveChanges();

            // ============================
            // Seed Bookings
            // ============================
            // ============================
            // Seed Bookings (ALSO USE NAVIGATION)
            // ============================
            var bookings = new List<Booking>
            {
                new Booking
                {
                    BookingDate = DateTime.Today,
                    EventDate = aiSummit.EventDate,
                    Event = aiSummit,
                    Venue = sandton
                },
                new Booking
                {
                    BookingDate = DateTime.Today.AddDays(-2),
                    EventDate = startup.EventDate,
                    Event = startup,
                    Venue = pretoria
                },
                new Booking
                {
                    BookingDate = DateTime.Today.AddDays(-7),
                    EventDate = summerMusic.EventDate,
                    Event = summerMusic,
                    Venue = capeTown
                },
                new Booking
                {
                    BookingDate = DateTime.Today.AddDays(-3),
                    EventDate = football.EventDate,
                    Event = football,
                    Venue = fnb
                }
            };

            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}