using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.Services.AntiFraud;

var builder = WebApplication.CreateBuilder(args);


// Register Anti-Fraud Rules (Strategy Pattern)
builder.Services.AddScoped<IFraudRule, GeoFencingRule>();
builder.Services.AddScoped<IFraudRule, DeviceSpoofingRule>();
builder.Services.AddScoped<IFraudRule, ImpossibleTravelRule>();
builder.Services.AddScoped<AntiFraudEngineService>();

// 1. Configure SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Configure Identity & Roles
builder.Services.AddDefaultIdentity<IdentityUser>(options => 
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 3. Configure MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Enable Authentication & Authorization Middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
