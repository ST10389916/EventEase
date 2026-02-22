using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventEase.Data;
using EventEase.Models;
using Azure.Storage.Blobs;


namespace EventEase.Controllers
{
    public class VenueController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public VenueController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Venues.ToListAsync());
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();
            var venue = _context.Venues.Find(id);
            if (venue == null) return NotFound();
            return View(venue);
        }
       

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Venue venue, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var containerName = _configuration["AzureStorage:ContainerName"];
                    var blobConnectionString = _configuration["AzureStorage:ConnectionString"];
                    var blobServiceClient = new BlobServiceClient(blobConnectionString);
                    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    await containerClient.CreateIfNotExistsAsync();

                    var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(image.FileName));
                    using (var stream = image.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, overwrite: true);
                    }

                    venue.ImageUrl = blobClient.Uri.ToString();
                }

                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Venue venue, IFormFile image)
        {
            if (id != venue.VenueId) return NotFound();

            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var containerName = _configuration["AzureStorage:ContainerName"];
                    var blobConnectionString = _configuration["AzureStorage:ConnectionString"];
                    var blobServiceClient = new BlobServiceClient(blobConnectionString);
                    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                    await containerClient.CreateIfNotExistsAsync();

                    var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(image.FileName));
                    using (var stream = image.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, overwrite: true);
                    }

                    venue.ImageUrl = blobClient.Uri.ToString();
                }

                _context.Update(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();
            return View(venue);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null) return NotFound();

            bool hasBookings = _context.Bookings.Any(b => b.VenueId == id);
            if (hasBookings)
            {
                ModelState.AddModelError("", "Cannot delete venue with active bookings.");
                return View(venue);
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
