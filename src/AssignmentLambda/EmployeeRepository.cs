using System.Threading.Tasks;
using AssignmentLambda.Entities;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace AssignmentLambda
{
    public class EmployeeRepository : IEmployeeRepository
    {

        public static MySqlConnection dbConnection;
        static string host = "assignment-rds.c5zwy39fcyex.ap-south-1.rds.amazonaws.com";
        static string id = "awsuser387";
        static string password = "password1234";
        static string port = "3306";
        static string database = "trial_database";

        string connectionString = string.Format("Server = {0};port={4};Database = {1}; User ID = {2}; Password = {3};", host, database, id, password, port);
        public async Task CreateEmployeeAsync(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee> GetEmployeeAsync(int employeeId)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateEmployeeAsync(int employeeId, Employee employee)
        {
            throw new System.NotImplementedException();
        }
    }
}