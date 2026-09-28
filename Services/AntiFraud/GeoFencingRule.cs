using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.Models;
using System;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Services.AntiFraud
{
    public class GeoFencingRule : IFraudRule
    {
        private const double MaxAllowedDistanceMeters = 200.0; // 200m tolerance perimeter

        public Task<FraudAlert> EvaluateAsync(CheckIn checkIn, Employee employee, ApplicationDbContext context)
        {
            if (!employee.BaseLocationLat.HasValue || !employee.BaseLocationLon.HasValue)
            {
                return Task.FromResult<FraudAlert>(null);
            }

            double distance = CalculateHaversineDistance(
                (double)checkIn.LocationLat, (double)checkIn.LocationLon,
                (double)employee.BaseLocationLat.Value, (double)employee.BaseLocationLon.Value);

            if (distance > MaxAllowedDistanceMeters)
            {
                return Task.FromResult(new FraudAlert
                {
                    RuleName = "Geo-Fencing Violation",
                    SeverityLevel = 3, // High Severity
                    Status = "Pending"
                });
            }

            return Task.FromResult<FraudAlert>(null);
        }

        private static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000; // Earth radius in meters
            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
    }
}
