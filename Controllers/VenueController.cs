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
        private readonly IWebHostEnvironment _environment;

        public VenueController(AppDbContext context, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;

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
            //if (ModelState.IsValid)
            //{
            //if (image != null && image.Length > 0)
            //{
            //    var containerName = _configuration["AzureStorage:ContainerName"];
            //    var blobConnectionString = _configuration["AzureStorage:ConnectionString"];
            //    var blobServiceClient = new BlobServiceClient(blobConnectionString);
            //    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            //    await containerClient.CreateIfNotExistsAsync();

            //    var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(image.FileName));
            //    using (var stream = image.OpenReadStream())
            //    {
            //        await blobClient.UploadAsync(stream, overwrite: true);
            //    }

            //    venue.ImageUrl = blobClient.Uri.ToString();
            //}


            if (ModelState.IsValid)
            {

                if (image != null && image.Length > 0)
                {
                    // Ensure Images folder exists
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate unique file name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Save file to wwwroot/Images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Save relative path to database
                    venue.ImageUrl = "/Images/" + fileName;
                }


                _context.Add(venue);
                await _context.SaveChangesAsync();
                //return View(venue);
                return RedirectToAction(nameof(Index));
                //}
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

            //if (ModelState.IsValid)
            //{
            //if (image != null && image.Length > 0)
            //{
            //    var containerName = _configuration["AzureStorage:ContainerName"];
            //    var blobConnectionString = _configuration["AzureStorage:ConnectionString"];
            //    var blobServiceClient = new BlobServiceClient(blobConnectionString);
            //    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            //    await containerClient.CreateIfNotExistsAsync();

            //    var blobClient = containerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(image.FileName));
            //    using (var stream = image.OpenReadStream())
            //    {
            //        await blobClient.UploadAsync(stream, overwrite: true);
            //    }

            //    venue.ImageUrl = blobClient.Uri.ToString();
            //}
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    // Ensure Images folder exists
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "Images");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate unique file name
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Save file to wwwroot/Images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Save relative path to database
                    venue.ImageUrl = "/Images/" + fileName;
                }

                _context.Update(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //}
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

            bool hasEvents = await _context.Events
                .AnyAsync(e => e.VenueId == id);

            bool hasBookings = await _context.Bookings
                .AnyAsync(b => b.VenueId == id);

            if (hasEvents || hasBookings)
            {
                TempData["ErrorMessage"] =
                    "Cannot delete this venue because it is linked to existing events or bookings.";
                return RedirectToAction(nameof(Index));
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Venue deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
