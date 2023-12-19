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
    public class EditBudget
    {
        private readonly IEditBudgetRunner runner;

        public EditBudget(IEditBudgetRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(EditBudget))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "EditBudget")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation(requestBody);
                EditBudgetRequest data = JsonConvert.DeserializeObject<EditBudgetRequest>(requestBody);

                var result = await this.runner.RunAsync(data);

                int statuscode;
                string statusMessage;
                Guid newGuid = Guid.NewGuid();

                if (result)
                {
                    statuscode = 200;
                    statusMessage = "Budget Edited successfully";
                }
                else
                {
                    statuscode = 400;
                    statusMessage = "Budget Edit fail";
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
                log.LogInformation($"Excepción en EditBudget: {ex}");
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
