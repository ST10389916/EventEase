using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Venue
    {
        public int VenueId { get; set; }
        public string? VenueName { get; set; }
        public string? Location { get; set; }
        public int? Capacity { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
