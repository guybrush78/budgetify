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
    internal class MapUsersToBudget
    {
        public readonly IMapUsersToBudgetRunner runner;

        public MapUsersToBudget(IMapUsersToBudgetRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(MapUsersToBudget))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "MapUsersToBudget")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                MapUsersToBudgetRequest data = JsonConvert.DeserializeObject<MapUsersToBudgetRequest>(requestBody);

                var result = await this.runner.RunAsync(data);

                int statuscode;
                string statusMessage;
                int userMapped = 0;

                if (result>0)
                {
                    statuscode = 200;
                    statusMessage = "Users Mapped successfully";
                    userMapped = result;
                }
                else
                {
                    statuscode = 400;
                    statusMessage = "User Mapped fail";
                }

                var jsonResponse = new
                {
                    statuscode,
                    statusMessage,
                    data = new { NumUserMapped = userMapped }
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };
            }
            catch (Exception ex)
            {
                log.LogInformation($"Excepción en MapUsersToBudget: {ex}");
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
