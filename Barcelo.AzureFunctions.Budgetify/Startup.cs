using Barcelo.AzureFunctions.Budgetify;
using Barcelo.AzureFunctions.Budgetify.Functions;
using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

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
