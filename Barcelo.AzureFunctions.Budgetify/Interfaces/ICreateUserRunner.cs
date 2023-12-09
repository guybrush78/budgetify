using Barcelo.AzureFunctions.Budgetify.Models;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Interfaces
{
    public interface ICreateUserRunner
    {
        Task<bool> RunAsync(CreateUserRequest req);
    }
}

