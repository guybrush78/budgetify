using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class UserBudgetVotingRequest
    {
        public string UserId { get; set; }
        public int BudgetId { get; set; }
        public int OptionId { get; set; }
    }
}
