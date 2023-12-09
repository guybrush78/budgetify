using Barcelo.AzureFunctions.Budgetify;
using Barcelo.AzureFunctions.Budgetify.Functions;
using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Barcelo.AzureFunctions.Budgetify
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<ICreateBudgetRunner, CreateBudgetRunner>();
            builder.Services.AddTransient<ICreateUserRunner, CreateUserRunner>();
        }
    }
}
