using MyRentalWebService.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<RepositoryResult<PaginatorDto<UserDto>>> GetAllUsers(int pageIndex, int pageSize);
        RepositoryResult<IEnumerable<RolesInUserOperationDto>> GetAllRoles();
        Task<RepositoryResult<UpdateUserDto>> GetUserByName(string userName);
        Task<RepositoryResult> CreateUser(CreateUserDto user);
        Task<RepositoryResult> UpdateUser(UpdateUserDto user);
    }
}
