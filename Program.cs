using Azure.Storage.Blobs;
using EventEase.Data;
using EventEaseWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Azure KeyVault (optional)
//// var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("vault")!);
//// builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add MVC
builder.Services.AddControllersWithViews();

// Blob Service
builder.Services.AddSingleton<BlobService>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// SESSION SUPPORT
builder.Services.AddSession();

// REQUIRED for Layout Session Access
builder.Services.AddHttpContextAccessor();

var app = builder.Build();


// -----------------------------
// HTTP PIPELINE
// -----------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

// SESSION must be after routing
app.UseSession();

app.UseAuthorization();


// -----------------------------
// ROUTING
// -----------------------------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


// -----------------------------
// DATABASE INITIALIZATION
// -----------------------------

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

app.Run();