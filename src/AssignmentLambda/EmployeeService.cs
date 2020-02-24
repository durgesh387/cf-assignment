using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using AssignmentLambda.Entities;
using Newtonsoft.Json;

namespace AssignmentLambda
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository = new EmployeeRepository();
        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            if (employee == null) throw new InvalidArgumentException(nameof(Employee), null);
            if (string.IsNullOrWhiteSpace(employee.Email)) throw new InvalidArgumentException(nameof(employee.Email), Convert.ToString(employee.Email));
            if (string.IsNullOrWhiteSpace(employee.FirstName)) throw new InvalidArgumentException(nameof(employee.Email), Convert.ToString(employee.Email));
            if (string.IsNullOrWhiteSpace(employee.Phone)) throw new InvalidArgumentException(nameof(employee.Email), Convert.ToString(employee.Email));
            if (string.IsNullOrWhiteSpace(employee.Designation)) throw new InvalidArgumentException(nameof(employee.Email), Convert.ToString(employee.Email));
            LambdaLogger.Log($"going to create employee with details as : {JsonConvert.SerializeObject(employee)}");
            return await _repository.CreateEmployeeAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0) throw new InvalidArgumentException(nameof(employeeId), Convert.ToString(employeeId));
            await _repository.DeleteEmployeeAsync(employeeId);
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0) throw new InvalidArgumentException(nameof(employeeId), Convert.ToString(employeeId));
            LambdaLogger.Log($"going to get employee with id as : {employeeId}");
            var employee = await _repository.GetEmployeeAsync(employeeId);
            if (employee == null) throw new EntityNotFoundException(nameof(Employee), employeeId);
            return employee;
        }

        public async Task UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            if (employeeId <= 0) throw new InvalidArgumentException(nameof(employeeId), Convert.ToString(employeeId));
            await _repository.UpdateEmployeeAsync(employeeId, employee);
        }
    }
}