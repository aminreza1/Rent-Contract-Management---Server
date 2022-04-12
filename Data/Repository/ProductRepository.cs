using System;
using System.Collections.Generic;
using System.Linq;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Data.Providers;
using MyRentalWebService.Infrastructure;
using MyRentalWebService.Infrastructure.ExtensionMethods;
using MyRentalWebService.Models;

namespace MyRentalWebService.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext context)
        {
            this._db = context;
        }

        public RepositoryResult<PaginatorDto<ProductItemDto>> GetAllProducts(int pageIndex, int pageSize)
        {
            var length = 0;

            var query = _db.Products
            .OrderByDescending(o => o.Id);

            length = query.Count();

            var items = query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(item => new ProductItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    RentPrice = item.RentPrice,
                    RentUnitText = item.RentUnit.ToString(),
                    RentUnit = (int)item.RentUnit,
                    Code = item.Code,
                    MaxUnitForRent = item.MaxUnitForRent,
                    MinUnitForRent = item.MinUnitForRent
                }).ToList();

            var data = new PaginatorDto<ProductItemDto>
            {
                Length = length,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items
            };
            return new RepositoryResult<PaginatorDto<ProductItemDto>>(200, data, null);
        }

        public RepositoryResult<ProductItemDto> GetProductById(int id)
        {
            var item = _db.Products.Find(id);
            if (item == null) 
                return new RepositoryResult<ProductItemDto>(404, null, "Product not found!");
            var data = new ProductItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Code = item.Code,
                RentPrice = item.RentPrice,
                RentUnitText = item.RentUnit.ToString(),
                RentUnit = (int)item.RentUnit
            };
            return new RepositoryResult<ProductItemDto>(200, data, null);
        }

        public RepositoryResult CreateProduct(CreateProductDto product)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(product.Name))
                    return new RepositoryResult(404, "Product name is required.");

                _db.Products.Add(new Product
                {
                    Name = product.Name,
                    Code = product.Code,
                    RentPrice = product.RentPrice,
                    RentUnit = (RentUnits)product.RentUnit
                });
                _db.SaveChanges();
                return new RepositoryResult(204, null);
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }

        }

        public RepositoryResult UpdateProduct(UpdateProductItemDto product)
        {
            try
            {
                var item = _db.Products.Find(product.Id);
                if (item == null)
                    return new RepositoryResult(404, "Product not found");

                item.Name = product.Name;
                item.Code = product.Code;
                item.RentPrice = product.RentPrice;
                item.RentUnit = (RentUnits)product.RentUnit;

                _db.SaveChanges();
                return new RepositoryResult(204, null); ;
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }

        public RepositoryResult ChangeProductActivation(ChangeProductActivationDto product)
        {
            try
            {
                // بررسی شود محصول در حین غیرفعال شدن در اجاره کسی نباشد

                var item = _db.Products.Find(product.Id);
                if (item == null)
                    return new RepositoryResult(404, "Product not found!");

                // _db.Products.Remove(item);
                item.IsActive = !item.IsActive;
                _db.SaveChanges();
                return new RepositoryResult(204, null);
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }
    }
}
