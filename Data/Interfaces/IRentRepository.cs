using System.Collections.Generic;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Infrastructure;

namespace MyRentalWebService.Data.Interfaces
{
    public interface IRentRepository
    {
        // Read (Select)(Get)
        RepositoryResult<PaginatorDto<RentDto>> GetAllRents(int pageIndex, int pageSize, string customerQuery);

        // Create (Get/Post)
        RepositoryResult<IEnumerable<RentItemDto>> GetProductsForAddingRentItems();
        RepositoryResult<IEnumerable<CustomerListInNewRentDto>> GetCustomersForAddNewRent();
        RepositoryResult AddNewRent(NewRentDto rentItem);

        // Update (Get/Post)
        RepositoryResult<RentOperationDto> GetRentById(int id);
        RepositoryResult TerminateRent(TerminateRentDto rentItem);

        // Delete (Post)
        // We dont have delete operation in this controller
    }
}