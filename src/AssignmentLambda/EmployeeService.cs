using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentLambda.Entities;

namespace AssignmentLambda
{
    public class EmployeeService : IEmployeeService
    {
        IEmployeeRepository _repository = new EmployeeRepository();
        public async Task CreateEmployeeAsync(Employee employee)
        {
            try
            {
                await _repository.CreateEmployeeAsync(employee);
            }
            catch (Exception e)
            {

            }
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                await _repository.DeleteEmployeeAsync(employeeId);
            }
            catch (Exception e)
            {

            }
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            try
            {
                return await _repository.GetEmployeeAsync(employeeId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            try
            {
                await _repository.UpdateEmployeeAsync(employeeId, employee);
            }
            catch (Exception e)
            {

            }
        }
    }
}