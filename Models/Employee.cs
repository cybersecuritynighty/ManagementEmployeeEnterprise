using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementEmployeeEnterprise.Models
{
    public class Employee
    {
        [Key]
        public Guid EmployeeId { get; set; } = Guid.NewGuid();
        
        [Required]
        public string IdentityUserId { get; set; }
        
        [Required, MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required, MaxLength(50)]
        public string LastName { get; set; }
        
        public Guid DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        
        public string RegisteredDeviceId { get; set; }
        
        // Navigation Properties
        public ICollection<CheckIn> CheckIns { get; set; }
    }

    public class CheckIn
    {
        [Key]
        public Guid CheckInId { get; set; } = Guid.NewGuid();
        
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        
        [Required]
        public DateTime CheckInTime { get; set; }
        
        [Column(TypeName = "decimal(9, 6)")]
        public decimal LocationLat { get; set; }
        
        [Column(TypeName = "decimal(9, 6)")]
        public decimal LocationLon { get; set; }
        
        public string CapturedDeviceId { get; set; }
        public bool IsFlagged { get; set; }
        
        public FraudAlert FraudAlert { get; set; }
    }

    public class FraudAlert
    {
        [Key]
        public Guid AlertId { get; set; } = Guid.NewGuid();
        
        public Guid CheckInId { get; set; }
        [ForeignKey("CheckInId")]
        public CheckIn CheckIn { get; set; }
        
        [Required]
        public string RuleName { get; set; }
        
        public int SeverityLevel { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class WorkspaceBooking
    {
        [Key]
        public Guid BookingId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid WorkspaceId { get; set; }
        [ForeignKey("WorkspaceId")]
        public Workspace Workspace { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required, MaxLength(20)]
        public string TimeSlot { get; set; } // 'FullDay', 'Morning', 'Afternoon'

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Confirmed"; // 'Confirmed', 'Cancelled'

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
