﻿using Barcelo.AzureFunctions.Budgetify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcelo.AzureFunctions.Budgetify.Interfaces
{
    public interface IMapUsersToBudgetRunner
    {        Task<int> RunAsync(MapUsersToBudgetRequest req);
    }
}
