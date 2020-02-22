using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using AssignmentLambda.Entities;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AssignmentLambda
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }

        IEmployeeService _employeeService = new EmployeeService();

        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The list of blogs</returns>
        public async Task<APIGatewayProxyResponse> GetAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Get Employee Request\n");
            string employeeIdString = null;
            int employeeId = 0;
            if (request.PathParameters != null && request.PathParameters.ContainsKey("employee_id"))
                employeeIdString = request.PathParameters["employee_id"];

            else if (request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("employee_id"))
                employeeIdString = request?.QueryStringParameters["employee_id"];
            if (Int32.TryParse(employeeIdString, out int num))
                employeeId = num;

            Employee employee;
            try
            {
                employee = await  _employeeService.GetEmployeeAsync(employeeId);
            }
            catch(InvalidArgumentException ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = ex.Message,
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }
            catch(EntityNotFoundException ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Body = ex.Message,
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(employee),
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        public async Task<APIGatewayProxyResponse> CreateAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Create Employee Request\n");
            var employee = request.Body != null ? JsonConvert.DeserializeObject<Employee>(request.Body) : null;
            //var employee = JsonConvert.DeserializeObject<Employee>(request.Body ?? "{\"message\": \"ERROR: No Payload\"}");
            int employeeId;
            try
            {
                employeeId = await _employeeService.CreateEmployeeAsync(employee);
            }
            catch(InvalidArgumentException ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = ex.Message,
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }
            
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                Body = $"employee created with Id : {employeeId}",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        public async Task<APIGatewayProxyResponse> UpdateAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Update Employee Request\n");
            string employeeIdString = null;
            int employeeId = 0;
            var employee = request.Body != null ? JsonConvert.DeserializeObject<Employee>(request.Body) : null;
            if (request.PathParameters != null && request.PathParameters.ContainsKey("employee_id"))
                employeeIdString = request.PathParameters["employee_id"];

            else if (request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("employee_id"))
                employeeIdString = request?.QueryStringParameters["employee_id"];
            if (Int32.TryParse(employeeIdString, out int num))
                employeeId = num;

            try{
               await _employeeService.UpdateEmployeeAsync(employeeId, employee);
            }
            catch(InvalidArgumentException ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = ex.Message,
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }    
            
            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "employee updated",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }

        public async Task<APIGatewayProxyResponse> DeleteAsync(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Delete Employee Request\n");
            string employeeIdString = null;
            int employeeId = 0;
            if (request.PathParameters != null && request.PathParameters.ContainsKey("employee_id"))
                employeeIdString = request.PathParameters["employee_id"];

            else if (request.QueryStringParameters != null && request.QueryStringParameters.ContainsKey("employee_id"))
                employeeIdString = request?.QueryStringParameters["employee_id"];
            if (Int32.TryParse(employeeIdString, out int num))
                employeeId = num;

            try
            {
                await _employeeService.DeleteEmployeeAsync(employeeId);
            }
            catch(InvalidArgumentException ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = ex.Message,
                    Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
                };
            }

            return new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "employee deleted",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
        }
    }
}
