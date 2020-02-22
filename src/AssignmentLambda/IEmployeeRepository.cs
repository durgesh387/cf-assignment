using System.Threading.Tasks;
using AssignmentLambda.Entities;

namespace AssignmentLambda
{
    public interface IEmployeeRepository
    {
        Task<int> CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(int employeeId, Employee employee);
        Task DeleteEmployeeAsync(int employeeId);
        Task<Employee> GetEmployeeAsync(int employeeId);
    }
}