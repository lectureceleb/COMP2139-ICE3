using Microsoft.EntityFrameworkCore;
using ICE3.Data;
using Serilog;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the context to the service collection with a connection string
builder.Services.AddDbContext<ApplicationDbContext>( options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// Configure Serilog
//                     SL      SL      Default?     
// Logging Level:    Verbose, Debug, Information, Warning, Errors, Fatal
// SL = short-lived
Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Debug()
    // .WriteTo.Console()
    // .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    // .Enrich.FromLogContext()
    .ReadFrom.Configuration(builder.Configuration)   // Take config settings from appsettings.json
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Project}/{action=Index}/{Id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{Id?}");

app.Run();