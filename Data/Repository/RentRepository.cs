using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Data.Providers;
using MyRentalWebService.Infrastructure;
using MyRentalWebService.Infrastructure.ExtensionMethods;
using MyRentalWebService.Models;

namespace MyRentalWebService.Data.Repository
{
    public class RentRepository : IRentRepository
    {
        private readonly AppDbContext _db;

        public RentRepository(AppDbContext context)
        {
            this._db = context;
        }
        public RepositoryResult<PaginatorDto<RentDto>> GetAllRents(int pageIndex, int pageSize, string customerQuery)
        {
            var splitCustomerQuery = customerQuery.Split("-");
            var customerId = splitCustomerQuery.Length >= 2 ? 
                splitCustomerQuery[1].SafeToInt(0) : 0;

            var length = 0;
            var hasFilter = false;
            var filters = new List<FilterDto>();

            var query = _db.Rents.AsQueryable();

            if (customerId != 0)
            {
                var customer = _db.Customers.Find(customerId);
                if(customer != null)
                {
                    query = query.Where(x => x.CustomerId == customerId);
                    hasFilter = true;
                    filters.Add(new FilterDto
                    {
                        Key = "customer",
                        ValueId = customer.Id,
                        Value = customer.FirstName + " " + customer.LastName
                    });
                }
                
            }
                

            length = query.Count();

            var dbRents = query
                .OrderByDescending(o => o.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToList();

            var items = new List<RentDto>();
            foreach (var dbRent in dbRents)
            {
                var customerFullName = "";
                var customerMobileNumber = "";
                var customer = _db.Customers.Find(dbRent.CustomerId);
                if (customer != null)
                {
                    customerFullName = customer.FirstName + " " + customer.LastName;
                    customerMobileNumber = customer.Mobile;
                }

                var rentDto = new RentDto
                {
                    Id = dbRent.Id,
                    CustomerFullName = customerFullName,
                    CustomerMobileNumber = customerMobileNumber,
                    RentDateTime = dbRent.RentDateTime.ToString("d"),
                    IsTerminated = dbRent.IsTerminated,
                    ReturnDateTime = dbRent.ReturnDateTime?.ToString("d") ?? "-",
                    FinalCost = dbRent.FinalCost ?? 0,
                    PredictedCost = dbRent.PredictedCost ?? 0,
                    Description = dbRent.Description
                };

                var productItems = _db.RentItems
                .Include(x => x.Product)
                .Where(x => x.RentId == dbRent.Id);

                rentDto.Items = productItems.Select(x => new RentItemDto
                {
                    ProductId = x.Product.Id,
                    ProductName = x.Product.Name,
                    RentId = x.Rent.Id,
                    PriceWhenRenting = x.Product.RentPrice,
                    UnitWhenRenting = x.Product.RentUnit.ToString()

                }).ToList();
                items.Add(rentDto);
            }

            var data = new PaginatorDto<RentDto>
            {
                Length = length,
                PageIndex = pageIndex,
                PageSize = pageSize,    
                Items = items,
                HasFilter = hasFilter,
                Filters = filters
            };

            return new RepositoryResult<PaginatorDto<RentDto>>(200, data, null);
        }

        public RepositoryResult<RentOperationDto> GetRentById(int id)
        {
            try
            {
                var rent = _db.Rents.Find(id);
                if (rent == null)
                    return
                        new RepositoryResult<RentOperationDto>(404, null, "Rent not found!");

                //////////////////////
                // Begin Customer Info
                var customerFullName = "";
                var customerMobileNumber = "";
                var customer = _db.Customers.Find(rent.CustomerId);
                if (customer != null)
                {
                    customerFullName = customer.FirstName + " " + customer.LastName;
                    customerMobileNumber = customer.Mobile;
                }
                // End Customer Info
                ///////////////////////

                var totalCalculatedCost = 0;
                var resultRentItems = new List<RentOperationItemDto>();

                var rentItems = _db.RentItems
                    .Include(x => x.Product)
                    .Where(i => i.RentId == rent.Id);
                if (rentItems.Any())
                {
                    resultRentItems = rentItems
                   .Select(rentItem => new RentOperationItemDto
                   {
                       RentId = rent.Id,
                       ProductId = rentItem.Product.Id,
                       ProductName = rentItem.Product.Name,
                       RentPrice = rentItem.Product.RentPrice,
                       RentUnitText = rentItem.Product.RentUnit.ToString(),
                       PriceWhenRenting = rentItem.PriceWhenRenting,
                       UnitWhenRenting = rentItem.UnitWhenRenting.ToString(),
                       CalculatedCost = CalculatePrice(rentItem, rent.RentDateTime, rent.IsTerminated, rent.ReturnDateTime)
                   }).ToList();
                    totalCalculatedCost = resultRentItems.Select(x => x.CalculatedCost).Sum();
                }

                var data = new RentOperationDto
                {
                    Id = rent.Id,
                    CustomerFullName = customerFullName,
                    CustomerMobileNumber = customerMobileNumber,
                    RentDateTime = rent.RentDateTime.ToString(),
                    ReturnDateTime = rent.ReturnDateTime.ToString() ?? "-",
                    FinalCost = rent.FinalCost ?? 0,
                    PredictedCost = rent.PredictedCost ?? 0,
                    Description = rent.Description,
                    Items = resultRentItems,
                    CalculatedCost = totalCalculatedCost,
                    IsTerminated = rent.IsTerminated
                };

                return new RepositoryResult<RentOperationDto>(200, data, null);
            }
            catch (Exception e)
            {
                return new RepositoryResult<RentOperationDto>(500, null, e.ToMessageResult());
            }


        }

        public RepositoryResult<IEnumerable<RentItemDto>> GetProductsForAddingRentItems()
        {
            var result = _db.Products.Select(x => new RentItemDto
            {
                ProductId = x.Id,
                ProductName = x.Name,
                PriceWhenRenting = x.RentPrice,
                UnitWhenRenting = x.RentUnit.ToString(),
                RentId = 0
            }).ToList();
            return new RepositoryResult<IEnumerable<RentItemDto>>(200, result, null);

        }
        public RepositoryResult<IEnumerable<CustomerListInNewRentDto>> GetCustomersForAddNewRent()
        {
            var result = _db.Customers.Select(customer => new CustomerListInNewRentDto
            {
                Id = customer.Id,
                FullName = customer.FirstName + ' ' + customer.LastName,
                Mobile = customer.Mobile
            }).ToList();
            return new RepositoryResult<IEnumerable<CustomerListInNewRentDto>>(200, result, null);
        }
        public RepositoryResult AddNewRent(NewRentDto rentItem)
        {
            try
            {
                var customer = _db.Customers.Find(rentItem.CustomerId);
                if (customer == null)
                    return new RepositoryResult(404, "Customer not found!");

                var newRent = new Rent
                {
                    RentDateTime = DateTime.Now,
                    CustomerId = customer.Id,
                    Description = rentItem.Description,
                    PredictedCost = rentItem.PredictedCost
                };
                _db.Rents.Add(newRent);
                _db.SaveChanges();

                foreach (var id in rentItem.ProductIds)
                {
                    var product = _db.Products.Find(id);
                    if (product == null) continue;

                    _db.RentItems.Add(new RentItem
                    {
                        ProductId = product.Id,
                        RentId = newRent.Id,
                        PriceWhenRenting = product.RentPrice,
                        UnitWhenRenting = product.RentUnit
                    });
                }

                _db.SaveChanges();
                return new RepositoryResult(204, null);
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }

        public RepositoryResult TerminateRent(TerminateRentDto rent)
        {
            try
            {
                var item = _db.Rents.Find(rent.Id);
                if (item == null)
                    return new RepositoryResult(404, "Rent contract not found!");

                item.IsTerminated = true;
                item.ReturnDateTime = DateTime.Now;
                item.FinalCost = rent.FinalCost;
                _db.SaveChanges();
                return new RepositoryResult(204, null);
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }

        private static int CalculatePrice(RentItem rentItem, DateTime rentDateTime, bool isTerminated, DateTime? returnDateTime)
        {
            var returnDt = !isTerminated ? DateTime.Now : (returnDateTime ?? DateTime.Now);
            var elapsedMinutes = (returnDt - rentDateTime).TotalMinutes;
            double elapsedUnits = 0;
            switch (rentItem.UnitWhenRenting)
            {
                case RentUnits.Daily:
                    elapsedUnits = elapsedMinutes / 1440;
                    break;
                case RentUnits.Hourly:
                    elapsedUnits = elapsedMinutes / 60;
                    break;
                case RentUnits.Monthly:
                    elapsedUnits = elapsedMinutes / 43200;
                    break;
                case RentUnits.Weekly:
                    elapsedUnits = elapsedMinutes / 10080;
                    break;
                default:
                    break;
            }

            return (int)(elapsedUnits * rentItem.PriceWhenRenting);
        }
    }
}