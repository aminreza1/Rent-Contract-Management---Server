using System.Collections.Generic;
//using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;

namespace MyRentalWebService.Controllers
{
    [ApiController]
    [Route("api/rent")]
    
    public class RentController : ControllerBase
    {
        private readonly IRentRepository _repo;
        private string _problemTitle = "InternalServerError";

        public RentController(IRentRepository repository)
        {
            _repo = repository;
        }

        // api/rent/list
        [HttpGet("list")]
        [Authorize]
        public ActionResult<PaginatorDto<RentDto>> GetAllRents(int pageIndex, int pageSize,string customerQuery)
        { 
            var repoRes = _repo.GetAllRents(pageIndex, pageSize, customerQuery);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }

        // api/rent/find/1
        [HttpGet("find/{id}")]
        [Authorize]
        public ActionResult<RentOperationDto> GetRentById(int id)
        {
            var repoRes = _repo.GetRentById(id);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }

        // api/rent/products
        [HttpGet("products")]
        [Authorize]
        public ActionResult<IEnumerable<RentItemDto>> GetProductsForAddingRentItems()
        {
            var repoRes = _repo.GetProductsForAddingRentItems();
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }

        // api/rent/customers
        [HttpGet("customers")]
        [Authorize]
        public ActionResult<IEnumerable<CustomerListInNewRentDto>> GetCustomersForAddNewRent()
        {
            var repoRes = _repo.GetCustomersForAddNewRent();
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }


        // api/rent/add
        [HttpPost("add")]
        [Authorize]
        public ActionResult AddNewRent(NewRentDto rent)
        {
            var repoRes = _repo.AddNewRent(rent);
            return repoRes.StatusCode switch
            {
                200 => Ok(),
                204 => NoContent(),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500, title: _problemTitle),
            };
        }


         // api/rent/terminate
        [HttpPost("terminate")]
        [Authorize]
        public ActionResult TerminateRent(TerminateRentDto rent)
        {
            var repoRes = _repo.TerminateRent(rent);
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