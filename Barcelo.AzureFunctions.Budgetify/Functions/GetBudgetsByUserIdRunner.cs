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
    public class GetBudgetsByUserIdRunner : IGetBudgetsByUserIdRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetBudgetsByUserIdRunner> _log;
        public GetBudgetsByUserIdRunner(IConfiguration configuration, ILogger<GetBudgetsByUserIdRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<List<UserBudgetTable>> RunAsync(string UserId)
        {
            try
            {
                _log.LogInformation($"Operación GetBudgetsByUserIdRunner iniciada. {UserId}");
                var repo = new BudgetifyRepository(_configuration, _log);
                var saveResult = await repo.GetBudgetsByUserId(int.Parse(UserId));
                return saveResult;
            }
            catch (Exception ex)
            {
                _log.LogError($"Excepcion en GetBudgetsByUserIdRunner {UserId}: {ex}");
                return null;
            }
        }
    }
}
