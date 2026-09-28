using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagementEmloyeeEnterprise.Data;
using ManagementEmloyeeEnterprise.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ManagementEmloyeeEnterprise.Controllers
{
    [Authorize(Roles = "Admin,HR")]
    public class FraudAlertsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FraudAlertsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FraudAlerts
        public async Task<IActionResult> Index()
        {
            var alerts = await _context.FraudAlerts
                .Include(f => f.CheckIn)
                .ThenInclude(c => c.Employee)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            return View(alerts);
        }

        // GET: FraudAlerts/Review/5
        public async Task<IActionResult> Review(Guid id)
        {
            var alert = await _context.FraudAlerts
                .Include(f => f.CheckIn)
                .ThenInclude(c => c.Employee)
                .FirstOrDefaultAsync(f => f.AlertId == id);

            if (alert == null) return NotFound();

            var viewModel = new FraudAlertReviewViewModel
            {
                AlertId = alert.AlertId,
                EmployeeName = $"{alert.CheckIn.Employee.FirstName} {alert.CheckIn.Employee.LastName}",
                RuleName = alert.RuleName,
                SeverityLevel = alert.SeverityLevel,
                CheckInTime = alert.CheckIn.CheckInTime,
                LocationLat = alert.CheckIn.LocationLat,
                LocationLon = alert.CheckIn.LocationLon,
                CapturedDeviceId = alert.CheckIn.CapturedDeviceId,
                RegisteredDeviceId = alert.CheckIn.Employee.RegisteredDeviceId
            };

            return View(viewModel);
        }

        // POST: FraudAlerts/ProcessReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessReview(FraudAlertReviewViewModel model)
        {
            var alert = await _context.FraudAlerts
                .Include(f => f.CheckIn)
                .FirstOrDefaultAsync(f => f.AlertId == model.AlertId);

            if (alert == null) return NotFound();

            var reviewerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            alert.Status = model.ActionStatus;
            alert.ReviewedBy = Guid.Parse(reviewerId);

            // If dismissed, unflag the underlying check-in record
            if (model.ActionStatus == "Dismissed")
            {
                alert.CheckIn.IsFlagged = false;
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Alert status updated to {model.ActionStatus}.";

            return RedirectToAction(nameof(Index));
        }
    }
}
