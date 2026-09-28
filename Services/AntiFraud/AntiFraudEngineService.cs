using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Services.AntiFraud
{
    public class AntiFraudEngineService
    {
        private readonly IEnumerable<IFraudRule> _rules;
        private readonly ApplicationDbContext _context;

        public AntiFraudEngineService(IEnumerable<IFraudRule> rules, ApplicationDbContext context)
        {
            _rules = rules;
            _context = context;
        }

        public async Task ProcessCheckInAsync(CheckIn checkIn, Employee employee)
        {
            foreach (var rule in _rules)
            {
                var alert = await rule.EvaluateAsync(checkIn, employee, _context);
                if (alert != null)
                {
                    alert.CheckInId = checkIn.CheckInId;
                    alert.CreatedAt = DateTime.UtcNow;

                    checkIn.IsFlagged = true;
                    _context.FraudAlerts.Add(alert);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
