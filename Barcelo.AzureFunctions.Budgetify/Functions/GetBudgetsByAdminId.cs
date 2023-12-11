using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class GetBudgetsByAdminId
    {
        private readonly IGetBudgetsByAdminIdRunner runner;

        public GetBudgetsByAdminId(IGetBudgetsByAdminIdRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(GetBudgetsByAdminId))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GetBudgetsByAdminId")] HttpRequest req,
            ILogger log)
        {
            int statuscode;
            string statusMessage;
            string jsonResult;
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                string data = JsonConvert.DeserializeObject<string>(requestBody);

                var result = await this.runner.RunAsync(data);

                if (result == null || !result.Any()) 
                {
                    statuscode = 200;
                    statusMessage = "Empty List";
                    jsonResult = "{}";
                }
                else
                {
                    statuscode = 200;
                    statusMessage = "Not Empty List";
                    jsonResult = JsonConvert.SerializeObject(result);
                }

                var jsonResponse = new
                {
                    statuscode = statuscode,
                    statusMessage = statusMessage,
                    data = jsonResult
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };
            }
            catch (Exception ex)
            {
                log.LogInformation($"Excepción in GetBudgetsByAdminId: {ex}");
                statuscode = 500;
                statusMessage = "GetBudgetsByAdminId Excepcion";
                jsonResult = "{}";
                var jsonResponse = new
                {
                    statuscode,
                    statusMessage,
                    data = jsonResult
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };
            }
        }
    }
}
