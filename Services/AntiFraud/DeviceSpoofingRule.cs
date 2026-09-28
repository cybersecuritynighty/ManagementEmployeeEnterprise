using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.Models;
using System;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Services.AntiFraud
{
    public class DeviceSpoofingRule : IFraudRule
    {
        public Task<FraudAlert> EvaluateAsync(CheckIn checkIn, Employee employee, ApplicationDbContext context)
        {
            if (string.IsNullOrEmpty(employee.RegisteredDeviceId))
            {
                return Task.FromResult<FraudAlert>(null);
            }

            if (!string.Equals(checkIn.CapturedDeviceId, employee.RegisteredDeviceId, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(new FraudAlert
                {
                    RuleName = "Unregistered Device Spoofing",
                    SeverityLevel = 2, // Medium Severity
                    Status = "Pending"
                });
            }

            return Task.FromResult<FraudAlert>(null);
        }
    }
}
