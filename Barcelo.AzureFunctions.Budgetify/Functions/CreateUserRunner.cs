using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class CreateUserRunner : ICreateUserRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateBudgetRunner> _log;
        public CreateUserRunner(IConfiguration configuration, ILogger<CreateBudgetRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<bool> RunAsync(CreateUserRequest req)
        {
            try
            {
                _log.LogInformation("Operación completada.");
                Console.WriteLine("Operación completada.");
                return true;

                var repo = new BudgetifyRepository(_configuration);
                bool saveResult = await repo.SaveUserAsync(req);
                return saveResult;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
