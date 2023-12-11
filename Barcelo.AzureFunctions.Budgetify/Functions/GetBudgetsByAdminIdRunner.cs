using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class GetBudgetsByAdminIdRunner : IGetBudgetsByAdminIdRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetBudgetsByAdminIdRunner> _log;
        public GetBudgetsByAdminIdRunner(IConfiguration configuration, ILogger<GetBudgetsByAdminIdRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<List<BudgetTable>> RunAsync(string AdminId)
        {
            try
            {
                _log.LogInformation($"Operación GetBudgetsByAdminIdRunner iniciada. {AdminId}");
                var repo = new BudgetifyRepository(_configuration, _log);
                var saveResult = await repo.GetBudgetsByAdminId(int.Parse(AdminId));
                return saveResult;
            }
            catch (Exception ex)
            {
                _log.LogError($"Excepcion en GetBudgetsByAdminIdRunner {AdminId}: {ex}");
                return null;
            }
        }
    }
}