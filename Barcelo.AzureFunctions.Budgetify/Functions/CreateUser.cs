using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class CreateUser
    {
        private readonly ICreateUserRunner runner;

        public CreateUser(ICreateUserRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(CreateUser))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CreateUser")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Inicio de Function CreateUser");
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateUserRequest data = JsonConvert.DeserializeObject<CreateUserRequest>(requestBody);

                var result = await this.runner.RunAsync(data);

                int statuscode;
                string statusMessage;
                Guid newGuid = Guid.NewGuid();

                if (result)
                {
                    statuscode = 200;
                    statusMessage = "User created successfully";
                } 
                else 
                {
                    statuscode = 400;
                    statusMessage = "User created fail";
                }

                var jsonResponse = new 
                {
                    statuscode,
                    statusMessage,
                    data = new { token = newGuid }
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };

            }
            catch (Exception ex)
            {
                log.LogInformation($"Excepción en CreateUser: {ex}");
                int statuscode = 500;
                string statusMessage = "Excepcion en el servicio";
                Guid newGuid = Guid.NewGuid();
                var jsonResponse = new
                {
                    statuscode,
                    statusMessage,
                    data = new { token = newGuid }
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };
            }
        }
    }
}