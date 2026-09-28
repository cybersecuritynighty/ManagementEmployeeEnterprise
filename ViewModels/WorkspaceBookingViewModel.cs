using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ManagementEmployeeEnterprise.ViewModels
{
    public class WorkspaceBookingViewModel
    {
        [Required]
        public Guid WorkspaceId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        [Required]
        public string TimeSlot { get; set; } = "FullDay";
    }

    public class OptimizationReportViewModel
    {
        public Guid WorkspaceId { get; set; }
        public string WorkspaceName { get; set; }
        public int Capacity { get; set; }
        public decimal CostPerSqFt { get; set; }
        public int TotalBookingsThisMonth { get; set; }
        public double UtilizationRatePercentage { get; set; }
        public decimal WasteCostMonthly { get; set; }
        public string OptimizationRecommendation { get; set; }
    }
}
