using Barcelo.AzureFunctions.Budgetify.Models;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Interfaces
{
    public interface ILoginRunner
    {
        Task<string> RunAsync(LoginRequest req);
    }
}
