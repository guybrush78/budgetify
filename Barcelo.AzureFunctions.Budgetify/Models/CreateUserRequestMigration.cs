using System;
using System.Collections.Generic;
using System.Text;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class CreateUserRequestMigration
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string DNI { get; set; }
        public string Email { get; set; }
    }
}
