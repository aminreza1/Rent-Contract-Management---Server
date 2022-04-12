using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyRentalWebService.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Controllers
{
    [ApiController]
    [Route("api/setup")]
    [AllowAnonymous]
    public class SetupController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ISetupRepository _repo;
        public SetupController(
            IConfiguration configuration, 
            ISetupRepository repository)
        {
            _configuration = configuration;
            _repo = repository;
        }

        [HttpGet("initial/admin")]
        // api/setup/initial/admin
        public  async Task<IActionResult> ConfigureAdminUser(string key)
        {
            var _key = _configuration.GetValue<String>("InitialSecurityKey");
            if (_key != key) return BadRequest();

            var result = await _repo.ConfigAdminUser();
            return Ok(result);
        }

        [HttpGet("initial/option")]
        // api/setup/initial/option
        public  async Task<IActionResult> ConfigureOptions(string key)
        {
            var _key = _configuration.GetValue<String>("InitialSecurityKey");
            if (_key != key) return BadRequest();

            var result = await _repo.ConfigOptions();
            return Ok(result);
        }
    }
}
