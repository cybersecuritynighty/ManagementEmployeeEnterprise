using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagementEmloyeeEnterprise.Data;
using ManagementEmloyeeEnterprise.Models;
using ManagementEmloyeeEnterprise.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ManagementEmloyeeEnterprise.Controllers
{
    [Authorize]
    public class CheckInController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckInController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CheckIn/Index
        // Displays attendance history. Employees see their own; HR/Admin see all.
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            
            var query = _context.CheckIns.Include(c => c.Employee).AsQueryable();

            if (userRole == "Employee")
            {
                query = query.Where(c => c.Employee.IdentityUserId == userId);
            }

            var checkIns = await query.OrderByDescending(c => c.CheckInTime).ToListAsync();
            return View(checkIns);
        }

        // GET: CheckIn/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CheckIn/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CheckInRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.IdentityUserId == identityUserId);

            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "Employee record not linked to this account.");
                return View(model);
            }

            var checkIn = new CheckIn
            {
                EmployeeId = employee.EmployeeId,
                CheckInTime = DateTime.UtcNow,
                LocationLat = model.LocationLat,
                LocationLon = model.LocationLon,
                CapturedDeviceId = model.CapturedDeviceId,
                CheckInMethod = model.CheckInMethod,
                IsFlagged = false // Will be updated in Phase 5 by the Anti-Fraud Engine
            };

            _context.CheckIns.Add(checkIn);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Check-in recorded successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
