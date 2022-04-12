using MyRentalWebService.Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Interfaces
{
    public interface ICustomerRepository
    {
        RepositoryResult<PaginatorDto<CustomerDto>> GetAllCustomers(int pageIndex, int pageSize);
        RepositoryResult<CustomerDto> GetCustomerById(int id);
        RepositoryResult CreateCustomer(CreateCustomerDto product);
        RepositoryResult UpdateCustomer(UpdateCustomerDto product);
    }
}
