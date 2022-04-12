using MyRentalWebService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Interfaces
{
    public interface ISetupRepository
    {
         Task<bool> ConfigAdminUser();
         Task<bool> ConfigOptions();
    }
}
