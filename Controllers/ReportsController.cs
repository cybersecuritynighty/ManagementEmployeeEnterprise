using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManagementEmployeeEnterprise.Data;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Controllers
{
    [Authorize(Roles = "Admin,HR,Manager")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reports/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reports/ExportAttendanceCsv
        public async Task<IActionResult> ExportAttendanceCsv()
        {
            var checkIns = await _context.CheckIns
                .Include(c => c.Employee)
                .OrderByDescending(c => c.CheckInTime)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("CheckInId,EmployeeName,TimestampUTC,Method,Latitude,Longitude,IsFlagged");

            foreach (var record in checkIns)
            {
                csv.AppendLine($"\"{record.CheckInId}\",\"{record.Employee.FirstName} {record.Employee.LastName}\",\"{record.CheckInTime:yyyy-MM-dd HH:mm:ss}\",\"{record.CheckInMethod}\",\"{record.LocationLat}\",\"{record.LocationLon}\",\"{record.IsFlagged}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"Attendance_Report_{DateTime.UtcNow:yyyyMMdd}.csv");
        }

        // GET: Reports/ExportFraudAuditCsv
        public async Task<IActionResult> ExportFraudAuditCsv()
        {
            var alerts = await _context.FraudAlerts
                .Include(f => f.CheckIn)
                .ThenInclude(c => c.Employee)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("AlertId,EmployeeName,RuleTriggered,SeverityLevel,Status,DetectedAtUTC");

            foreach (var alert in alerts)
            {
                csv.AppendLine($"\"{alert.AlertId}\",\"{alert.CheckIn.Employee.FirstName} {alert.CheckIn.Employee.LastName}\",\"{alert.RuleName}\",\"{alert.SeverityLevel}\",\"{alert.Status}\",\"{alert.CreatedAt:yyyy-MM-dd HH:mm:ss}\"");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", $"Fraud_Audit_Report_{DateTime.UtcNow:yyyyMMdd}.csv");
        }
    }
}
