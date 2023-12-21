using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class UserBudgetTable : BudgetTable
    {
        public int? BudgetOption { get; set; }
    }
}
