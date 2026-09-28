using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class RealEstateOptimizationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int MonthlyWorkingDays = 22; // Standard 22 working days per month

        public RealEstateOptimizationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var startDate = DateTime.UtcNow.AddDays(-30);
            var workspaces = await _context.Workspaces.Where(w => w.IsActive).ToListAsync();
            var reports = new List<OptimizationReportViewModel>();

            foreach (var ws in workspaces)
            {
                var bookingsCount = await _context.WorkspaceBookings
                    .CountAsync(b => b.WorkspaceId == ws.WorkspaceId 
                                  && b.BookingDate >= startDate 
                                  && b.Status == "Confirmed");

                double totalAvailableSlots = ws.Capacity * MonthlyWorkingDays;
                double utilizationRate = totalAvailableSlots > 0 
                    ? (bookingsCount / totalAvailableSlots) * 100 
                    : 0;

                // Estimate idle capacity financial leakage (Assumes 50 sq ft per workstation)
                decimal totalMonthlyCost = ws.CostPerSqFt * 50 * ws.Capacity;
                decimal wasteCost = totalMonthlyCost * (decimal)((100 - utilizationRate) / 100);

                string recommendation = utilizationRate switch
                {
                    < 35 => "Critical Underutilization: Consider downsizing or subleasing capacity.",
                    < 65 => "Moderate Usage: Convert fixed desks to shared hybrid flex-desks.",
                    _ => "Optimal Utilization: Space meets efficiency benchmarks."
                };

                reports.Add(new OptimizationReportViewModel
                {
                    WorkspaceId = ws.WorkspaceId,
                    WorkspaceName = ws.Name,
                    Capacity = ws.Capacity,
                    CostPerSqFt = ws.CostPerSqFt,
                    TotalBookingsThisMonth = bookingsCount,
                    UtilizationRatePercentage = Math.Round(utilizationRate, 2),
                    WasteCostMonthly = Math.Round(wasteCost, 2),
                    OptimizationRecommendation = recommendation
                });
            }

            return View(reports);
        }
    }
}
