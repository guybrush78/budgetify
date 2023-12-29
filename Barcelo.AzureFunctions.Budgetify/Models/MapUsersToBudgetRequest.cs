using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class MapUsersToBudgetRequest
    {
        public int BudgetId { get; set; }
        public List<string> UserList { get; set; }
    }
}
