using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;


namespace EventEase.Controllers
{
    public class BookingDetailController : Controller
    {
        private readonly AppDbContext _context;

        public BookingDetailController(AppDbContext context)
        {
            _context = context;
        }

        // GET: BookingDetail
        public async Task<IActionResult> Index(bool? isAvailable, int? eventTypeId, DateTime? startDate, DateTime? endDate)
        {
            var query = from b in _context.Bookings
                        join v in _context.Venues on b.VenueId equals v.VenueId
                        join e in _context.Events on b.EventId equals e.EventId
                        join et in _context.EventTypes on e.EventTypeId equals et.EventTypeId into eventTypeJoin
                        from eventType in eventTypeJoin.DefaultIfEmpty()
                        select new BookingDetailView
                        {
                            BookingId = b.BookingId,
                            BookingDate = b.BookingDate.Value.Date,
                            EventDate = b.EventDate.Value.Date,
                            VenueId = v.VenueId,
                            VenueName = v.VenueName,
                            Location = v.Location,
                            Capacity = v.Capacity.Value,
                            ImageUrl = v.ImageUrl,
                            IsAvailable = v.IsAvailable,
                            EventId = e.EventId,
                            EventName = e.EventName,
                            EventDescription = e.Description,
                            EventTypeId = eventType != null ? eventType.EventTypeId : (int?)null,
                            EventTypeName = eventType != null ? eventType.Name : ""
                        };
            if (isAvailable.HasValue)
                query = query.Where(b => b.IsAvailable == isAvailable.Value);

            ViewData["SelectedAvailability"] = isAvailable;
            ViewData["EventTypes"] = new SelectList(await _context.EventTypes.ToListAsync(), "EventTypeId", "Name");

            if (startDate.HasValue)
                query = query.Where(q => q.EventDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(q => q.EventDate <= endDate.Value);

            if (eventTypeId.HasValue)
                query = query.Where(q => q.EventTypeId == eventTypeId.Value); 
            
            var eventTypes = await _context.EventTypes.ToListAsync();
            ViewBag.EventTypes = new SelectList(eventTypes, "EventTypeId", "Name");

            return View(await query.ToListAsync());
        }
    }
}

