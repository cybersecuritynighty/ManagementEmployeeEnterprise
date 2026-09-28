using System;
using System.ComponentModel.DataAnnotations;

namespace ManagementEmployeeEnterprise.ViewModels
{
    public class FraudAlertReviewViewModel
    {
        public Guid AlertId { get; set; }
        public string EmployeeName { get; set; }
        public string RuleName { get; set; }
        public int SeverityLevel { get; set; }
        public DateTime CheckInTime { get; set; }
        public decimal LocationLat { get; set; }
        public decimal LocationLon { get; set; }
        public string CapturedDeviceId { get; set; }
        public string RegisteredDeviceId { get; set; }

        [Required]
        public string ActionStatus { get; set; } // 'ConfirmedFraud' or 'Dismissed'
    }
}
