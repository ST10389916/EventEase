
namespace EventEase.Models
{
    public class BookingDetailView
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime EventDate { get; set; }

        public int VenueId { get; set; }
        public string VenueName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }

        public int EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }

        public int? EventTypeId { get; set; }
        public string EventTypeName { get; set; } 
    }
}
