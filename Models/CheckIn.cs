using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementEmployeeEnterprise.Models
{
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
}
