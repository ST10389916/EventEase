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
            // GET: Event
            public async Task<IActionResult> Index()
            {
                var events = _context.Events.Include(e => e.EventType);
                return View(await events.ToListAsync());
            }

            // GET: Event/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null) return NotFound();

                var eventItem = await _context.Events
                    .Include(e => e.EventType)
                    .FirstOrDefaultAsync(m => m.EventId == id);
                if (eventItem == null) return NotFound();

                return View(eventItem);
            }

            // GET: Event/Create
            public IActionResult Create()
            {
                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name");
                return View();
            }

            // POST: Event/Create
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create([Bind("EventId,EventName,Description,EventDate,EventTypeId")] Event eventItem)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(eventItem);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
                return View(eventItem);
            }

            // GET: Event/Edit/5
            public async Task<IActionResult> Edit(int? id)
            {
                if (id == null) return NotFound();

                var eventItem = await _context.Events.FindAsync(id);
                if (eventItem == null) return NotFound();

                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
                return View(eventItem);
            }

            // POST: Event/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,Description,EventDate,EventTypeId")] Event eventItem)
            {
                if (id != eventItem.EventId) return NotFound();

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(eventItem);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EventExists(eventItem.EventId)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["EventTypeId"] = new SelectList(_context.EventTypes, "EventTypeId", "Name", eventItem.EventTypeId);
                return View(eventItem);
            }

            // GET: Event/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null) return NotFound();

                var eventItem = await _context.Events
                    .Include(e => e.EventType)
                    .FirstOrDefaultAsync(m => m.EventId == id);
                if (eventItem == null) return NotFound();

                return View(eventItem);
            }

            // POST: Event/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var eventItem = await _context.Events.FindAsync(id);
                if (eventItem != null)
                {
                    _context.Events.Remove(eventItem);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

            private bool EventExists(int id)
            {
                return _context.Events.Any(e => e.EventId == id);
            }
        }
    }

