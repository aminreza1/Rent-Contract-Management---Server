using System.Collections.Generic;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Infrastructure;
using MyRentalWebService.Models;

namespace MyRentalWebService.Data.Interfaces
{
    public interface IProductRepository
    {
        RepositoryResult<PaginatorDto<ProductItemDto>> GetAllProducts(int pageIndex, int pageSize);
        RepositoryResult<ProductItemDto> GetProductById(int id);
        RepositoryResult CreateProduct(CreateProductDto product);
        RepositoryResult UpdateProduct(UpdateProductItemDto product);
        RepositoryResult ChangeProductActivation(ChangeProductActivationDto product);
    }
}
