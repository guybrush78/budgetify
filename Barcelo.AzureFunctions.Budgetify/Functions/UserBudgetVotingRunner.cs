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
    public class UserBudgetVotingRunner : IUserBudgetVotingRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserBudgetVotingRunner> _log;
        public UserBudgetVotingRunner(IConfiguration configuration, ILogger<UserBudgetVotingRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<string> RunAsync(UserBudgetVotingRequest req)
        {
            try
            {
                _log.LogInformation("Operación EditBudgetRunner iniciada.");

                var repo = new BudgetifyRepository(_configuration, _log);
                string saveResult = await repo.UserBudgetVoting(req);
                return saveResult;
            }
            catch (Exception ex)
            {
                _log.LogError($"UserBudgetVotingRunner Exception: {ex}");
                return string.Empty;
            }
        }

    }
}
