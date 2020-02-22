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
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
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
            var result = _employeeService.GetEmployeeAsync(employeeId);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "I am an employee",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }

        public APIGatewayProxyResponse Create(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogLine("Create Employee Request\n");
            var employee = request.Body != null ? JsonConvert.DeserializeObject<Employee>(request.Body) : null;
            //var employee = JsonConvert.DeserializeObject<Employee>(request.Body ?? "{\"message\": \"ERROR: No Payload\"}");
            _employeeService.CreateEmployeeAsync(employee);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                Body = "employee created",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };
            return response;
        }

        public APIGatewayProxyResponse Update(APIGatewayProxyRequest request, ILambdaContext context)
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
            _employeeService.UpdateEmployeeAsync(employeeId, employee);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "employee updated",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }

        public APIGatewayProxyResponse Delete(APIGatewayProxyRequest request, ILambdaContext context)
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
            _employeeService.DeleteEmployeeAsync(employeeId);

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "employee deleted",
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }
    }
}
