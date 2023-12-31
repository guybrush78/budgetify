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
    public class UpdateProposal
    {
        private readonly IUpdateProposalRunner runner;

        public UpdateProposal(IUpdateProposalRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(UpdateProposal))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "UpdateProposal")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation(requestBody);
                UpdateProposalRequest data = JsonConvert.DeserializeObject<UpdateProposalRequest>(requestBody);

                var result = await this.runner.RunAsync(data);

                int statuscode;
                string statusMessage;
                string token = string.Empty;

                if (!string.IsNullOrEmpty(result))
                {
                    statuscode = 200;
                    statusMessage = "Update Proposal successfully";
                    token = result;
                }
                else
                {
                    statuscode = 400;
                    statusMessage = "Update Proposal fail";
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
                log.LogInformation($"Excepción en UpdateProposal: {ex}");
                int statuscode = 500;
                string statusMessage = "Excepcion en el servicio";
                
                var jsonResponse = new
                {
                    statuscode,
                    statusMessage,
                    data = new { string.Empty }
                };

                return new JsonResult(jsonResponse)
                {
                    StatusCode = statuscode
                };
            }
        }
    }
}
