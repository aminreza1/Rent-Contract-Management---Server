using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Models;

namespace MyRentalWebService.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;
        private string _problemTitle = "InternalServerError";

        public ProductController(IProductRepository repository)
        {
            _repo = repository;
        }

        // api/product/list?pageIndex=0&pageSize=10
        [HttpGet("list")]
        [Authorize]
        public ActionResult<PaginatorDto<ProductItemDto>> GetAllProducts(int pageIndex,int pageSize)
        {
            var repoRes = _repo.GetAllProducts(pageIndex, pageSize);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }

        // api/product/find/1
        [HttpGet("find/{id}")]
        [Authorize]
        public ActionResult<ProductItemDto> GetProductById(int id) {
            var repoRes = _repo.GetProductById(id);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message,statusCode:500),
            };
        }


        // api/product/create
        [HttpPost("create")]
        [Authorize]
        public ActionResult CreateProduct(CreateProductDto product)
        {
            var repoRes = _repo.CreateProduct(product);
            return repoRes.StatusCode switch
            {
                200 => Ok(),
                204 => NoContent(),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500,title:_problemTitle),
            };
        }

        // api/product/update
        [HttpPost("update")]
        [Authorize]
        public ActionResult UpdateProduct(UpdateProductItemDto product)
        {
            var repoRes = _repo.UpdateProduct(product);
            return repoRes.StatusCode switch
            {
                200 => Ok(),
                204 => NoContent(),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500, title: _problemTitle),
            };
           
        }

        // api/product/change/activation
        [HttpPost("change/activation")]
        [Authorize]
        public ActionResult InactiveProduct(ChangeProductActivationDto product)
        {
            var repoRes = _repo.ChangeProductActivation(product);
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
