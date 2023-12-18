using Barcelo.AzureFunctions.Budgetify.Models;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Interfaces
{
    public interface IEditBudgetRunner
    {
        Task<bool> RunAsync(EditBudgetRequest req);
    }
}
