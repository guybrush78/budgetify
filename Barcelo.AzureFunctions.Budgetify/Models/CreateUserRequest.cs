using System;
using System.Collections.Generic;
using System.Text;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class CreateUserRequest
    {
        public int Type { get; set; }
        public string Name { get; set; }
        public string DNI { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Token { get; set; }
    }
}
