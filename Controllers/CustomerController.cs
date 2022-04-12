
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using System.Collections.Generic;

namespace MyRentalWebService.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _repo;
        private string _problemTitle = "InternalServerError";

        public CustomerController(ICustomerRepository repository)
        {
            _repo = repository;
        }

        // api/customer/list?pageIndex=0&pageSize=10
        [HttpGet("list")]
        [Authorize]
        public ActionResult<PaginatorDto<CustomerDto>> GetAllCustomers(int pageIndex, int pageSize)
        {
            var repoRes = _repo.GetAllCustomers(pageIndex, pageSize);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }


        // api/customer/find/1
        [HttpGet("find/{id}")]
        [Authorize]
        public ActionResult<CustomerDto> GetCustomerById(int id)
        {
            var repoRes = _repo.GetCustomerById(id);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }


        // api/customer/create
        [HttpPost("create")]
        [Authorize]
        public ActionResult CreateCustomer(CreateCustomerDto product)
        {
            var repoRes = _repo.CreateCustomer(product);
            return repoRes.StatusCode switch
            {
                200 => Ok(),
                204 => NoContent(),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500, title: _problemTitle),
            };
        }

        // api/customer/update
        [HttpPost("update")]
        [Authorize]
        public ActionResult UpdateCustomer(UpdateCustomerDto product)
        {
            var repoRes = _repo.UpdateCustomer(product);
            return repoRes.StatusCode switch
            {
                200 => Ok(),
                204 => NoContent(),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500, title: _problemTitle),
            };
        }
    }
}
