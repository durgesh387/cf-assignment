using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentLambda.Entities;
using MySql.Data;

namespace AssignmentLambda
{
    public class EmployeeService : IEmployeeService
    {
        public async Task CreateEmployeeAsync(Employee employee)
        {
            // throw new System.NotImplementedException();
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            // throw new System.NotImplementedException();
        }

        public async Task<Employee> GetEmployeeAsync(int EmployeeId)
        {
            return new Employee();
        }

        public async Task UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            // throw new System.NotImplementedException();
        }
    }
}