using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class UpdateProposalRunner : IUpdateProposalRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UpdateProposalRunner> _log;
        public UpdateProposalRunner(IConfiguration configuration, ILogger<UpdateProposalRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<string> RunAsync(UpdateProposalRequest req)
        {
            try
            {
                _log.LogInformation("Operación EditBudgetRunner iniciada.");

                var repo = new BudgetifyRepository(_configuration, _log);
                string saveResult = await repo.UpdateProposal(req);
                return saveResult;
            }
            catch (Exception ex)
            {
                _log.LogError($"UpdateProposalRunner Exception: {ex}");
                return string.Empty;
            }
        }

    }
}
