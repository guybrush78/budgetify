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
    internal class MapUsersToBudgetRunner:IMapUsersToBudgetRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MapUsersToBudgetRunner> _log;
        public MapUsersToBudgetRunner(IConfiguration configuration, ILogger<MapUsersToBudgetRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<int> RunAsync(MapUsersToBudgetRequest req)
        {
            try
            {
                _log.LogInformation("Operación MapUsersToBudgetRunner iniciada.");

                var repo = new BudgetifyRepository(_configuration, _log);
                int NumUsers = await repo.MapUsersToBudget(req);
                return NumUsers;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
