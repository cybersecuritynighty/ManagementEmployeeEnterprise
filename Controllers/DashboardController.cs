using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Controllers
{
    [Authorize(Roles = "Admin,HR,Manager")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalEmployees = await _context.Employees.CountAsync(),
                TotalDepartments = await _context.Departments.CountAsync(),
                ActiveWorkspaces = await _context.Workspaces.CountAsync(w => w.IsActive),
                PendingFraudAlerts = await _context.FraudAlerts.CountAsync(f => f.Status == "Pending"),
                RecentAlerts = await _context.FraudAlerts
                    .Include(f => f.CheckIn)
                    .ThenInclude(c => c.Employee)
                    .Where(f => f.Status == "Pending")
                    .OrderByDescending(f => f.CreatedAt)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }
    }
}
