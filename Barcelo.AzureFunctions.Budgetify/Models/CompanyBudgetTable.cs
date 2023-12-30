using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class CompanyBudgetTable : BudgetTable
    {
        public string BudgetWin { get; set; }
        public string AutenticationToken { get; set; }
        public string ProposalFile { get; set; }
        public string CompanyId { get; set; }
    }
}