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
    public class UserBudgetVoting
    {
        private readonly IUserBudgetVotingRunner runner;

        public UserBudgetVoting(IUserBudgetVotingRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(UserBudgetVoting))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "UserBudgetVoting")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation(requestBody);
                UserBudgetVotingRequest data = JsonConvert.DeserializeObject<UserBudgetVotingRequest>(requestBody);

                var result = await this.runner.RunAsync(data);

                int statuscode;
                string statusMessage;
                string token;

                if (!string.IsNullOrEmpty(result))
                {
                    statuscode = 200;
                    statusMessage = "Voting successfully";
                    token = result;
                }
                else
                {
                    statuscode = 400;
                    statusMessage = "Voting fail";
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
                log.LogInformation($"Excepción en UserBudgetVoting: {ex}");
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
