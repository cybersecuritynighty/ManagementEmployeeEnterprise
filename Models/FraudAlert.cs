using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementEmployeeEnterprise.Models
{
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
}
