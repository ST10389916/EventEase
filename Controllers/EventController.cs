using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventEase.Models;
using EventEase.Data;

namespace EventEase.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }

        // =============================
        // GET: Event
        // =============================
        public async Task<IActionResult> Index()
        {
            var events = _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Venue);

            return View(await events.ToListAsync());
        }

        // =============================
        // GET: Event/Details/5
        // =============================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null) return NotFound();

            return View(eventItem);
        }

        // =============================
        // GET: Event/Create
        // =============================
        public IActionResult Create()
        {
            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name");
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName");
            return View();
        }

        // =============================
        // POST: Event/Create
        // =============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event eventItem)
        {
            bool venueConflict = await _context.Events
                .AnyAsync(e =>
                    e.VenueId == eventItem.VenueId &&
                    e.EventDate.Value.Date == eventItem.EventDate.Value.Date);

            if (venueConflict)
            {
                ModelState.AddModelError("EventDate",
                    "This venue already has an event scheduled on this date.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
                ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventItem.VenueId);
                return View(eventItem);
            }

            _context.Add(eventItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event created successfully.";
            return RedirectToAction(nameof(Index));
        }

        // =============================
        // GET: Event/Edit/5
        // =============================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound();

            ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventItem.VenueId);

            return View(eventItem);
        }

        // =============================
        // POST: Event/Edit/5
        // =============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event eventItem)
        {
            if (id != eventItem.EventId)
                return NotFound();

            bool venueConflict = await _context.Events
                .AnyAsync(e =>
                    e.EventId != eventItem.EventId &&
                    e.VenueId == eventItem.VenueId &&
                    e.EventDate.Value.Date == eventItem.EventDate.Value.Date);

            if (venueConflict)
            {
                ModelState.AddModelError("EventDate",
                    "This venue already has another event scheduled on this date.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
                ViewData["VenueId"] = new SelectList(_context.Venues, "VenueId", "VenueName", eventItem.VenueId);
                return View(eventItem);
            }

            try
            {
                _context.Update(eventItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.EventId == eventItem.EventId))
                    return NotFound();
                else
                    throw;
            }

            TempData["SuccessMessage"] = "Event updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        // =============================
        // GET: Event/Delete/5
        // =============================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var eventItem = await _context.Events
                .Include(e => e.EventType)
                .Include(e => e.Venue)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventItem == null) return NotFound();

            return View(eventItem);
        }

        // =============================
        // POST: Event/Delete/5
        // =============================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
                return NotFound();

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.EventId == id);

            if (hasBookings)
            {
                TempData["ErrorMessage"] =
                    "Cannot delete this event because it has associated bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Event deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}