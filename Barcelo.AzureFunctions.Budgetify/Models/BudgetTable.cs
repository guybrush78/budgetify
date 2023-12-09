using System;
using System.Collections.Generic;
using System.Text;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class BudgetTable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? OrganizationId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateTime ProposalFrom { get; set; }
        public DateTime ProposalTo { get; set; }
        public byte[] ContractFile { get; set; }
        public string ContractName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
