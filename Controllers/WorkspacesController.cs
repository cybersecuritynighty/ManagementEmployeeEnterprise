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
    public class WorkspacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkspacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Workspaces
        public async Task<IActionResult> Index()
        {
            var workspaces = await _context.Workspaces.Where(w => w.IsActive).ToListAsync();
            return View(workspaces);
        }

        // GET: Workspaces/Book/5
        public async Task<IActionResult> Book(Guid id)
        {
            var workspace = await _context.Workspaces.FindAsync(id);
            if (workspace == null || !workspace.IsActive)
            {
                return NotFound();
            }

            ViewBag.WorkspaceName = workspace.Name;
            ViewBag.Capacity = workspace.Capacity;

            return View(new WorkspaceBookingViewModel { WorkspaceId = id });
        }

        // POST: Workspaces/Book
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(WorkspaceBookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.IdentityUserId == userId);

            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "Employee profile not found.");
                return View(model);
            }

            var workspace = await _context.Workspaces.FindAsync(model.WorkspaceId);
            if (workspace == null)
            {
                return NotFound();
            }

            // Check existing bookings for capacity limits on the given date
            var existingBookingsCount = await _context.WorkspaceBookings
                .CountAsync(b => b.WorkspaceId == model.WorkspaceId 
                              && b.BookingDate.Date == model.BookingDate.Date 
                              && b.Status == "Confirmed");

            if (existingBookingsCount >= workspace.Capacity)
            {
                ModelState.AddModelError(string.Empty, "Workspace is fully booked for the selected date.");
                ViewBag.WorkspaceName = workspace.Name;
                ViewBag.Capacity = workspace.Capacity;
                return View(model);
            }

            var booking = new WorkspaceBooking
            {
                WorkspaceId = model.WorkspaceId,
                EmployeeId = employee.EmployeeId,
                BookingDate = model.BookingDate.Date,
                TimeSlot = model.TimeSlot,
                Status = "Confirmed"
            };

            _context.WorkspaceBookings.Add(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Workspace desk booked successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
