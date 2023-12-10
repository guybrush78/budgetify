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
    public class Login
    {
        private readonly ILoginRunner runner;

        public Login(ILoginRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(Login))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Login")] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("Inicio de Function Login");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                LoginRequest data = JsonConvert.DeserializeObject<LoginRequest>(requestBody);

                string sessionId = await this.runner.RunAsync(data);

                int statuscode;
                string statusMessage;
                string token;

                if (!string.IsNullOrEmpty(sessionId))
                {
                    statuscode = 200;
                    statusMessage = "Login successfully";
                    token = sessionId;
                }
                else
                {
                    statuscode = 400;
                    statusMessage = "Login fail";
                    token = string.Empty;
                }

                var jsonResponse = new
                {
                    statuscode,
                    statusMessage,
                    data = new { token }
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };

            }
            catch (Exception ex)
            {
                log.LogInformation($"Excepción en Login: {ex}");
                int statuscode = 500;
                string statusMessage = "Excepcion en el servicio";
                var jsonResponse = new
                {
                    statuscode,
                    statusMessage,
                    data = new { token = string.Empty }
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };
            }
        }
    }
}