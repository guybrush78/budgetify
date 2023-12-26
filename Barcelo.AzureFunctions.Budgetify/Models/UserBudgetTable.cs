using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class UserBudgetTable : BudgetTable
    {
        public string BudgetOption { get; set; }
        public string AutenticationToken { get; set; }
    }
}
