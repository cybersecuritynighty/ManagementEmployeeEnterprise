using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementEmployeeEnterprise.Models
{
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
