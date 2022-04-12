using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly string _problemTitle = "InternalServerError";
        public UserController(IUserRepository repository)
        {
            _repo = repository;
        }

        // api/user/list?pageIndex=0&pageSize=10
        [HttpGet("list")]
        [Authorize]
        public async Task<ActionResult<PaginatorDto<UserDto>>> GetAllUsers(int pageIndex, int pageSize)
        {
            var repoRes = await _repo.GetAllUsers(pageIndex, pageSize);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500,title: _problemTitle),
            };
        }

        // api/user/find/admin
        [HttpGet("find/{name}")]
        [Authorize]
        public async Task<ActionResult<UpdateUserDto>> GetUserByName(string name)
        {
            var repoRes = await _repo.GetUserByName(name);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }

        // api/user/roles
        [HttpGet("roles")]
        [Authorize]
        public ActionResult<IEnumerable<RolesInUserOperationDto>> GetAllRoles()
        {
            var repoRes =  _repo.GetAllRoles();
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500, title: _problemTitle),
            };
        }

        // api/user/create
        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateUser(CreateUserDto user)
        {
            var repoRes = await _repo.CreateUser(user);
            return repoRes.StatusCode switch
            {
                200 => Ok(),
                204 => NoContent(),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500, title: _problemTitle),
            };
        }

        // api/user/update
        [HttpPost("update")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateUser(UpdateUserDto user)
        {
            var repoRes = await _repo.UpdateUser(user);
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
