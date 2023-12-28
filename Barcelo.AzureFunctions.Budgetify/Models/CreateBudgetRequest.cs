using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class CreateBudgetRequest
    {
        [Required]
        public int UserId { get; set; }

        public int? OrganizationId { get; set; }

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

        [StringLength(1000)]
        public string ContractFile { get; set; }

        [StringLength(100)]
        public string ContractName { get; set; }

        public List<string> Options { get; set; }
    }
}
