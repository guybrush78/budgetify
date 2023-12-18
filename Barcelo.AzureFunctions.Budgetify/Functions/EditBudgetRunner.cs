using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class EditBudgetRunner : IEditBudgetRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EditBudgetRunner> _log;
        public EditBudgetRunner(IConfiguration configuration, ILogger<EditBudgetRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<bool> RunAsync(EditBudgetRequest req)
        {
            try
            {
                _log.LogInformation("Operación EditBudgetRunner iniciada.");

                var repo = new BudgetifyRepository(_configuration, _log);
                bool saveResult = await repo.EditBudget(req);
                return saveResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
