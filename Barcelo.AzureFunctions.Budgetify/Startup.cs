using Barcelo.AzureFunctions.Budgetify;
using Barcelo.AzureFunctions.Budgetify.Functions;
using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Barcelo.AzureFunctions.Budgetify
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IGetOptionsByBudgetIdRunner, GetOptionsByBudgetIdRunner>();
            builder.Services.AddTransient<IGetBudgetsByAdminIdRunner, GetBudgetsByAdminIdRunner>();
            builder.Services.AddTransient<ICreateBudgetRunner, CreateBudgetRunner>();
            builder.Services.AddTransient<IEditBudgetRunner, EditBudgetRunner>();
            builder.Services.AddTransient<ICreateUserRunner, CreateUserRunner>();
            builder.Services.AddTransient<ILoginRunner, LoginRunner>();
            builder.Services.AddTransient<BudgetifyRepository>();
        }
    }
}
