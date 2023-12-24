using Barcelo.AzureFunctions.Budgetify.Functions;
using Barcelo.AzureFunctions.Budgetify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Interfaces
{
    public interface IUserBudgetVotingRunner
    {
        Task<string> RunAsync(UserBudgetVotingRequest req);
    }
}
