using Microsoft.EntityFrameworkCore;
using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Services.AntiFraud
{
    public class ImpossibleTravelRule : IFraudRule
    {
        private const double MaxFeasibleSpeedKmH = 200.0; // Max reasonable transit speed

        public async Task<FraudAlert> EvaluateAsync(CheckIn checkIn, Employee employee, ApplicationDbContext context)
        {
            // Retrieve previous check-in within the last 12 hours
            var previousCheckIn = await context.CheckIns
                .Where(c => c.EmployeeId == employee.EmployeeId && c.CheckInId != checkIn.CheckInId)
                .OrderByDescending(c => c.CheckInTime)
                .FirstOrDefaultAsync();

            if (previousCheckIn == null) return null;

            double timeDifferenceHours = (checkIn.CheckInTime - previousCheckIn.CheckInTime).TotalHours;
            if (timeDifferenceHours <= 0 || timeDifferenceHours > 12) return null;

            double distanceKm = CalculateHaversineDistance(
                (double)checkIn.LocationLat, (double)checkIn.LocationLon,
                (double)previousCheckIn.LocationLat, (double)previousCheckIn.LocationLon) / 1000.0;

            double calculatedSpeed = distanceKm / timeDifferenceHours;

            if (calculatedSpeed > MaxFeasibleSpeedKmH)
            {
                return new FraudAlert
                {
                    RuleName = $"Impossible Travel Velocity ({Math.Round(calculatedSpeed)} km/h)",
                    SeverityLevel = 3, // Critical Severity
                    Status = "Pending"
                };
            }

            return null;
        }

        private static double CalculateHaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371000;
            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            return R * (2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
        }
    }
}
