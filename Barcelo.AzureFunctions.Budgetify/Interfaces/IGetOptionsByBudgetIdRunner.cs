using Barcelo.AzureFunctions.Budgetify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Interfaces
{
    public interface IGetOptionsByBudgetIdRunner
    {
        Task<List<BudgetOptionsTable>> RunAsync(string BudgetId);
    }
}
