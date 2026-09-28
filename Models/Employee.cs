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
}
