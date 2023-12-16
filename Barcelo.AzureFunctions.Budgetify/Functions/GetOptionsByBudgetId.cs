using Barcelo.AzureFunctions.Budgetify.Interfaces;
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
    public class GetOptionsByBudgetId
    {
        private readonly IGetOptionsByBudgetIdRunner runner;

        public GetOptionsByBudgetId(IGetOptionsByBudgetIdRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(GetOptionsByBudgetId))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GetOptionsByBudgetId")] HttpRequest req,
            ILogger log)
        {
            int statuscode;
            string statusMessage;
            string jsonResult;
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"GetBudgetsByAdminId request: {requestBody}");
                BudgetIdModel data = JsonConvert.DeserializeObject<BudgetIdModel>(requestBody);

                string budgetId = data.BudgetId.ToString();
                var result = await this.runner.RunAsync(budgetId);

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
                    statuscode,
                    statusMessage,
                    data = jsonResult
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };
            }
            catch (Exception ex)
            {
                log.LogInformation($"Excepción in GetOptionsbyBudgetId: {ex}");
                statuscode = 500;
                statusMessage = "GetOptionsbyBudgetId Excepcion";
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
        private class BudgetIdModel
        {
            public int BudgetId { get; set; }
        }
    }
}
