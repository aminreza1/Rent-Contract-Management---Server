using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using System.Threading.Tasks;

namespace MyRentalWebService.Controllers
{
    [Route("api/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _repo;
        public AccountController(IAccountRepository repository)
        {
            _repo = repository;
        }

        [HttpPost]
        [Route("token")]
        // api/account/token
        public async Task<IActionResult> CreateToken(LoginDto info)
        {

            var repoRes = await _repo.CreateToken(info);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                401 => Unauthorized(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }

        [HttpPost]
        [Route("register")]
        // api/account/register
        public async Task<IActionResult> Register(SignUpDto newUserInfo)
        {
            var repoRes = await _repo.RegisterUser(newUserInfo);
            return repoRes.StatusCode switch
            {
                200 => Ok(repoRes.Data),
                400 => BadRequest(repoRes.Message),
                401 => Unauthorized(repoRes.Message),
                404 => NotFound(repoRes.Message),
                _ => Problem(repoRes.Message, statusCode: 500),
            };
        }
        

    }
}
