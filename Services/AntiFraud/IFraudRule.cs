using ManagementEmployeeEnterprise.Data;
using ManagementEmployeeEnterprise.Models;
using System.Threading.Tasks;

namespace ManagementEmployeeEnterprise.Services.AntiFraud
{
    public interface IFraudRule
    {
        Task<FraudAlert> EvaluateAsync(CheckIn checkIn, Employee employee, ApplicationDbContext context);
    }
}
