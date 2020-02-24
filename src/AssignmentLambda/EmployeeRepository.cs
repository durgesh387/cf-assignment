using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
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
        private static string ConnectionString = $"server={Host};user id={UserId}; password={Password};port={Port};database={Database}; ";

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            LambdaLogger.Log("inside the repo function");
            //try
            //{
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                LambdaLogger.Log("connection established");
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand())
                {
                    LambdaLogger.Log("setting variables");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.CommandText = Routines.CreateEmployee;
                    AddEmployeeCommandParams(command, employee);
                    var employeeId = await command.ExecuteScalarAsync();
                    LambdaLogger.Log($"employee created with id as : {Convert.ToInt32(employeeId)}");
                    return Convert.ToInt32(employeeId);
                }
            }
            //}
            // catch (Exception ex)
            // {
            //     LambdaLogger.Log(ex.ToString());
            //     LambdaLogger.Log(ex.Message);
            //     LambdaLogger.Log(ex.StackTrace);
            //     return 0;
            // }

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
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand())
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
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Connection = connection;
                    command.CommandText = Routines.UpdateEmployee;
                    command.Parameters.AddWithValue("vEmployeeId", employeeId);
                    using (DbDataReader dr = await command.ExecuteReaderAsync())
                    {
                        return await ReadEmployeeAsync(dr);
                    }
                }
            }
        }

        private async Task<Employee> ReadEmployeeAsync(DbDataReader dr)
        {
            Employee employee = null;
            if (await dr.ReadAsync())
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
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = new MySqlCommand())
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