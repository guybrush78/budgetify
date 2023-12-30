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
    public class GetBudgetsByCompanyIdRunner : IGetBudgetsByCompanyIdRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetBudgetsByCompanyIdRunner> _log;
        public GetBudgetsByCompanyIdRunner(IConfiguration configuration, ILogger<GetBudgetsByCompanyIdRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<List<CompanyBudgetTable>> RunAsync(string CompanyId)
        {
            try
            {
                _log.LogInformation($"Operación GetBudgetsByCompanyIdRunner iniciada. {CompanyId}");
                var repo = new BudgetifyRepository(_configuration, _log);
                var saveResult = await repo.GetBudgetsByCompanyId(int.Parse(CompanyId));
                return saveResult;
            }
            catch (Exception ex)
            {
                _log.LogError($"Excepcion en GetBudgetsByCompanyIdRunner {CompanyId}: {ex}");
                return null;
            }
        }
    }
}
