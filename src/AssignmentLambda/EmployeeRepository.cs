using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using AssignmentLambda.Entities;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AssignmentLambda
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const string Host = "assignment-rds.c5zwy39fcyex.ap-south-1.rds.amazonaws.com";
        private const string UserId = "awsuser387";
        private const string Password = "password1234";
        private const string Port = "3306";
        private const string Database = "trial_database";
        private static string ConnectionString = $"Server = {Host};port={Port};Database = {Database}; User ID = {UserId}; Password = {Password};";
        
        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using(MySqlCommand command = new MySqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.CommandText = Routines.CreateEmployee;
                    command.Parameters.AddWithValue("vId", Guid.NewGuid());
                    AddEmployeeCommandParams(command, employee);
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        private void AddEmployeeCommandParams(MySqlCommand command, Employee employee)
        {
            command.Parameters.AddWithValue("vFirstName", employee.FirstName);
            command.Parameters.AddWithValue("vLastName", employee.LastName);
            command.Parameters.AddWithValue("vEmail", employee.Email);
            command.Parameters.AddWithValue("vPhone", employee.Phone);
            command.Parameters.AddWithValue("vDesignation", employee.Designation);
            command.Parameters.AddWithValue("vAddress", employee.Address);
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using(MySqlCommand command = new MySqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.CommandText = Routines.DeleteEmployee;
                    command.Parameters.AddWithValue("vEmployeeId", employeeId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using(MySqlCommand command = new MySqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.CommandText = Routines.UpdateEmployee;
                    command.Parameters.AddWithValue("vEmployeeId", employeeId);
                    using(DbDataReader dr = await command.ExecuteReaderAsync())
                    {
                        return await ReadEmployeeAsync(dr);
                    }
                }
            }
        }

        private async Task<Employee> ReadEmployeeAsync(DbDataReader dr)
        {
            Employee employee = null;
            if(await dr.ReadAsync())
            {
                employee.EmployeeId = Convert.ToInt32(dr["employee_id"]);
                employee.FirstName = Convert.ToString(dr["first_name"]);
                employee.LastName = Convert.ToString(dr["last_name"]);
                employee.Email = Convert.ToString(dr["email"]);
                employee.Phone = Convert.ToString(dr["phone"]);
                employee.Designation = Convert.ToString(dr["designation"]);
                employee.Address = Convert.ToString(dr["address"]);
            }
            return employee;
        }

        public async Task UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            using(MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using(MySqlCommand command = new MySqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.CommandText = Routines.UpdateEmployee;
                    command.Parameters.AddWithValue("vEmployeeId", employeeId);
                    AddEmployeeCommandParams(command, employee);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}