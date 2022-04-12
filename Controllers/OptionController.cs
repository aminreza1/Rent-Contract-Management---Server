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
    [ApiController]
    [Route("api/option")]
    public class OptionController : ControllerBase
    {
        private readonly IOptionRepository _repo;
        private string _problemTitle = "InternalServerError";

        public OptionController(IOptionRepository repository)
        {
            _repo = repository;
        }

        // api/option/get
        [HttpGet("get")]
        [AllowAnonymous]
        public ActionResult<OptionDto> GetOptions()
        {
            var repoRes = _repo.GetOptions();
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500, title: _problemTitle),
            };
        }

        // api/option/update
        [HttpPost("update")]
        [Authorize]
        public ActionResult UpdateOptions(OptionDto opt)
        {
            var repoRes = _repo.UpdateOptions(opt);
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
