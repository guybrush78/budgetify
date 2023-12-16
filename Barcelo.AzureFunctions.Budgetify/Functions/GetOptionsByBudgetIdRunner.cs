using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class GetOptionsByBudgetIdRunner : IGetOptionsByBudgetIdRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetOptionsByBudgetIdRunner> _log;
        public GetOptionsByBudgetIdRunner(IConfiguration configuration, ILogger<GetOptionsByBudgetIdRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<List<BudgetOptionsTable>> RunAsync(string BudgetId)
        {
            try
            {
                _log.LogInformation($"Operación GetOptionsByBudgetIdRunner iniciada. {BudgetId}");
                var repo = new BudgetifyRepository(_configuration, _log);
                var saveResult = await repo.GetOptionsByBudgetId(int.Parse(BudgetId));
                return saveResult;
            }
            catch (Exception ex)
            {
                _log.LogError($"Excepcion en GetOptionsByBudgetIdRunner {BudgetId}: {ex}");
                return null;
            }
        }
    }
}
