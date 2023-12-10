using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class LoginRunner : ILoginRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginRunner> _log;
        public LoginRunner(IConfiguration configuration, ILogger<LoginRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<string> RunAsync(LoginRequest req)
        {
            try
            {
                _log.LogInformation($"Operación LoginRunner iniciada. {req.username}");

                var repo = new BudgetifyRepository(_configuration, _log);
                LoginResponse saveResult = await repo.LoginAsync(req);
                string sessionId = $"{saveResult.role}{saveResult.sessionId}";
                return sessionId;
            }
            catch (Exception ex)
            {
                _log.LogError($"Excepcion en LoginRunner {req.username}: {ex}");
                return string.Empty;
            }
        }
    }
}
