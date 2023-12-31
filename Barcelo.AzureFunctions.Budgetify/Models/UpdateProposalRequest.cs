using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class UpdateProposalRequest
    {
        public string CompanyId { get; set; }
        public int BudgetId { get; set; }
        public string ProposalContract { get; set; }
    }
}
