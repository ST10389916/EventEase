using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public int EventTypeId { get; set; }
        public int VenueId { get; set; }
        public string? EventName { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public EventType? EventType { get; set; }
        public Venue? Venue { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
    }
}
