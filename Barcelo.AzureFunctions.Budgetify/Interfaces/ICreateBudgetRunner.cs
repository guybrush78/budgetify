using Barcelo.AzureFunctions.Budgetify.Models;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Interfaces
{
    public interface ICreateBudgetRunner
    {
        Task<bool> RunAsync(CreateBudgetRequest req);
    }
}
