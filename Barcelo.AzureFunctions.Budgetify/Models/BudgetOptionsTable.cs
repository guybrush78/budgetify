namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class BudgetOptionsTable
    {
        //Id	BudgetId	OptionDescription
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public string OptionDescription { get; set;}
    }
}
