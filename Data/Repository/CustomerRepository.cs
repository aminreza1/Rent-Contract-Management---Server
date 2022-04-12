using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Data.Providers;
using MyRentalWebService.Infrastructure.ExtensionMethods;
using MyRentalWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRentalWebService.Data.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _db;

        public CustomerRepository(AppDbContext context)
        {
            this._db = context;
        }
        public RepositoryResult<PaginatorDto<CustomerDto>> GetAllCustomers(int pageIndex, int pageSize)
        {
            var length = 0;

            var query = _db.Customers
            .OrderByDescending(o => o.Id);

            length = query.Count();

            var items = new List<CustomerDto>();
            foreach (var item in query
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList())
            {
                var customer = new CustomerDto
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Mobile = item.Mobile,
                    NationalCode = item.NationalCode,
                    Phone = item.Phone,
                    Address = item.Address
                };
                var checkActiveRent = _db.Rents.Any(r => !r.IsTerminated && r.CustomerId == item.Id);
                customer.HasActiveRent = checkActiveRent;
                items.Add(customer);
            }

            var data = new PaginatorDto<CustomerDto>
            {
                Length = length,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items
            };

            return new RepositoryResult<PaginatorDto<CustomerDto>>(200, data, null);
        }

        public RepositoryResult<CustomerDto> GetCustomerById(int id)
        {
            var customer = _db.Customers.Find(id);
            if (customer == null)
                return new RepositoryResult<CustomerDto>(404, null, "Customer not found!");

            var _hasActiveRent =
                _db.Rents.Any(i =>
                i.CustomerId == customer.Id &&
                i.IsTerminated == false);

            var data = new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Mobile = customer.Mobile,
                NationalCode = customer.NationalCode,
                Phone = customer.Phone,
                Address = customer.Address,
                HasActiveRent = _hasActiveRent
            };
            return new RepositoryResult<CustomerDto>(200, data, null);
        }

        public RepositoryResult CreateCustomer(CreateCustomerDto customer)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customer.LastName))
                    return new RepositoryResult(400, "Customer last name is required.");
                if (string.IsNullOrWhiteSpace(customer.Mobile))
                    return new RepositoryResult(400, "Customer mobile is required.");

                _db.Customers.Add(new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Mobile = customer.Mobile,
                    Phone = customer.Phone,
                    NationalCode = customer.NationalCode,
                    Address = customer.Address
                });
                _db.SaveChanges();
                return new RepositoryResult(204, null);
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }
        public RepositoryResult UpdateCustomer(UpdateCustomerDto customer)
        {
            try
            {
                var item = _db.Customers.Find(customer.Id);
                if (item == null)
                    return new RepositoryResult(400, "Customer not found!");

                item.FirstName = customer.FirstName;
                item.LastName = customer.LastName;
                item.NationalCode = customer.NationalCode;
                item.Phone = customer.Phone;
                item.Mobile = customer.Mobile;
                item.Address = customer.Address;

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
