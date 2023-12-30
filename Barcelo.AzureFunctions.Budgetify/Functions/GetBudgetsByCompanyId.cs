﻿using Barcelo.AzureFunctions.Budgetify.Interfaces;
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
    public class GetBudgetsByCompanyId
    {
        private readonly IGetBudgetsByCompanyIdRunner runner;

        public GetBudgetsByCompanyId(IGetBudgetsByCompanyIdRunner runner)
        {
            this.runner = runner;
        }

        [FunctionName(nameof(GetBudgetsByCompanyId))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GetBudgetsByCompanyId")] HttpRequest req,
            ILogger log)
        {
            int statuscode;
            string statusMessage;
            string jsonResult;
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                log.LogInformation($"GetBudgetsByCompanyId request: {requestBody}");
                CompanyIdModel data = JsonConvert.DeserializeObject<CompanyIdModel>(requestBody);

                string CompanyId = data.CompanyId.ToString();
                var result = await this.runner.RunAsync(CompanyId);

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
                log.LogInformation($"Excepción in GetBudgetsByCompanyId: {ex}");
                statuscode = 500;
                statusMessage = "GetBudgetsByCompanyId Excepcion";
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
        private class CompanyIdModel
        {
            public int CompanyId { get; set; }
        }
    }

}