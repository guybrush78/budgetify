using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class CreateBudgetRunner : ICreateBudgetRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateBudgetRunner> _log;
        public CreateBudgetRunner(IConfiguration configuration, ILogger<CreateBudgetRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<bool> RunAsync(CreateBudgetRequest req)
        {
            try
            {
                _log.LogInformation("Operación CreateBudgetRunner completada.");
                Console.WriteLine("Operación CreateBudgetRunner completada2.");
                return true;

                var repo = new BudgetifyRepository(_configuration);
                bool saveResult = await repo.SaveBudget(req);
                return saveResult;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }
    }
}
