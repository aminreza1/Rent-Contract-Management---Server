using MyRentalWebService.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<RepositoryResult<TokenDto>> CreateToken(LoginDto info);
        Task<RepositoryResult<TokenDto>> RegisterUser(SignUpDto userInfo);
    }
}
