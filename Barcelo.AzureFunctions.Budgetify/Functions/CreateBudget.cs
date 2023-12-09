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
    public class CreateBudget
    {
        private readonly ICreateBudgetRunner runner;

        public CreateBudget(ICreateBudgetRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(CreateBudget))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "CreateBudget")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                CreateBudgetRequest data = JsonConvert.DeserializeObject<CreateBudgetRequest>(requestBody);
                
                var result = await this.runner.RunAsync(data);

                return result ? new OkResult() : (IActionResult)new BadRequestResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { Error = ex.Message });
            }
        }
    }
}
