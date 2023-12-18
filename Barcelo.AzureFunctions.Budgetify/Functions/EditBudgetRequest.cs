using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class EditBudgetRequest
    {
        [Required]
        public int BudgetId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        [Required]
        public DateTime ProposalFrom { get; set; }

        [Required]
        public DateTime ProposalTo { get; set; }

        public byte[] ContractFile { get; set; }

        [StringLength(100)]
        public string ContractName { get; set; }

        public Dictionary<string, string> Options { get; set; } //[{key:value},{key:value}...]
    }
}
