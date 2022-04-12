using MyRentalWebService.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Interfaces
{
    public interface IOptionRepository
    {
        RepositoryResult<OptionDto> GetOptions();
        RepositoryResult UpdateOptions(OptionDto option);
    }
}
