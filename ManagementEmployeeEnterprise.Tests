using Microsoft.EntityFrameworkCore;
using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.Models;
using ManagementEmployeeEnterprise.Services.AntiFraud;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ManagementEmployeeEnterprise.Tests
{
    public class AntiFraudEngineTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GeoFencingRule_ShouldTriggerAlert_WhenCheckInIsTooFarFromBase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var rule = new GeoFencingRule();

            var employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Doe",
                IdentityUserId = "user-123",
                BaseLocationLat = 11.5564m, // Office Latitude (Phnom Penh)
                BaseLocationLon = 104.9282m // Office Longitude
            };

            var checkInFarAway = new CheckIn
            {
                CheckInId = Guid.NewGuid(),
                EmployeeId = employee.EmployeeId,
                CheckInTime = DateTime.UtcNow,
                LocationLat = 13.3671m, // Siem Reap Latitude (~230km away)
                LocationLon = 103.8448m,
                CapturedDeviceId = "Device123",
                CheckInMethod = "GPS"
            };

            // Act
            var alert = await rule.EvaluateAsync(checkInFarAway, employee, context);

            // Assert
            Assert.NotNull(alert);
            Assert.Equal("Geo-Fencing Violation", alert.RuleName);
            Assert.Equal(3, alert.SeverityLevel);
        }

        [Fact]
        public async Task GeoFencingRule_ShouldNotTriggerAlert_WhenCheckInIsWithinTolerance()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var rule = new GeoFencingRule();

            var employee = new Employee
            {
                EmployeeId = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Smith",
                IdentityUserId = "user-456",
                BaseLocationLat = 11.556400m,
                BaseLocationLon = 104.928200m
            };

            var checkInNear = new CheckIn
            {
                CheckInId = Guid.NewGuid(),
                EmployeeId = employee.EmployeeId,
                CheckInTime = DateTime.UtcNow,
                LocationLat = 11.556450m, // ~5 meters away
                LocationLon = 104.928250m,
                CapturedDeviceId = "Device456",
                CheckInMethod = "GPS"
            };

            // Act
            var alert = await rule.EvaluateAsync(checkInNear, employee, context);

            // Assert
            Assert.Null(alert);
        }
    }
}
