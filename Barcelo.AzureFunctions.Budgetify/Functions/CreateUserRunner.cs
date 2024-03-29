﻿using Barcelo.AzureFunctions.Budgetify.Interfaces;
using Barcelo.AzureFunctions.Budgetify.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Functions
{
    public class CreateUserRunner : ICreateUserRunner
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CreateUserRunner> _log;
        public CreateUserRunner(IConfiguration configuration, ILogger<CreateUserRunner> log)
        {
            _configuration = configuration;
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }
        public async Task<bool> RunAsync(CreateUserRequest req)
        {
            try
            {
                _log.LogInformation($"Operación CreateUserRunner iniciada. {req.Email}");

                var repo = new BudgetifyRepository(_configuration, _log);
                bool saveResult = await repo.SaveUserAsync(req);
                return saveResult;
            }
            catch (Exception ex)
            {
                _log.LogError($"Excepcion en CreateUserRunner {req.Email}: {ex}");
                return false;
            }
        }
    }
}
