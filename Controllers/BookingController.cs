using EventEase.Data;
using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EventEaseWebApp.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }
        public async Task<IActionResult> Index()
        {
            var bookings = _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.Event);
            return View(await bookings.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
                ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
                return View(booking);
            }

            var _booking = await _context.Events
                                .FirstOrDefaultAsync(b =>
                                    b.VenueId == booking.VenueId);

            booking.EventDate = _booking?.EventDate;

            _context.Add(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
            ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Booking booking)
        {
            if (id != booking.BookingId)
                return NotFound();

            var _booking = await _context.Events
                                .FirstOrDefaultAsync(b => 
                                    b.VenueId == booking.VenueId);

            if (!ModelState.IsValid)
            {
                ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", booking.VenueId);
                ViewData["EventId"] = new SelectList(_context.Events, "EventId", "EventName", booking.EventId);
                return View(booking);
            }

            var existingBooking = await _context.Bookings.FindAsync(id);
            if (existingBooking == null)
                return NotFound();

            // Update only allowed fields
            existingBooking.BookingDate = booking.BookingDate;
            existingBooking.EventDate = _booking?.EventDate;
            existingBooking.VenueId = booking.VenueId;
            existingBooking.EventId = booking.EventId;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Venue)
                .Include(b => b.Event)
                .FirstOrDefaultAsync(m => m.BookingId == id);

            if (booking == null) return NotFound();

            return View(booking);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            // 🚫 Prevent deleting active/future bookings
            if (booking.EventDate >= DateTime.Today)
            {
                TempData["ErrorMessage"] =
                    "Cannot delete an active or upcoming booking.";
                return RedirectToAction(nameof(Index));
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Booking deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}



