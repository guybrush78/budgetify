using System;
using System.Collections.Generic;
using System.Text;

namespace Barcelo.AzureFunctions.Budgetify.Models
{
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}