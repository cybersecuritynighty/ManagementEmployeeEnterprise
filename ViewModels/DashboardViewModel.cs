using System.Collections.Generic;
using ManagementEmployeeEnterprise.Models;

namespace ManagementEmployeeEnterprise.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }
        public int TotalDepartments { get; set; }
        public int PendingFraudAlerts { get; set; }
        public int ActiveWorkspaces { get; set; }
        public IEnumerable<FraudAlert> RecentAlerts { get; set; }
    }
}
