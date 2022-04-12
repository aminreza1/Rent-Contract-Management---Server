using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Infrastructure;
using MyRentalWebService.Infrastructure.ExtensionMethods;
using MyRentalWebService.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        public AccountRepository(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<RepositoryResult<TokenDto>> CreateToken(LoginDto info)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(info.UserName);
                if (user == null)
                    return new RepositoryResult<TokenDto>(404, null, "UserName not found!");

                var loginResult = await _signInManager
                    .PasswordSignInAsync(info.UserName, info.Password, false, false);
                if (!loginResult.Succeeded)
                    return new RepositoryResult<TokenDto>(400, null, "Username or password is incorrect.");


                if (user.IsBlock)
                    return new RepositoryResult<TokenDto>(401, null, "The account has been blocked!");

                var generatedToken = await GenerateToken(user);
                if (generatedToken == null)
                    return new RepositoryResult<TokenDto>(401, null, "Access denied!");

                return new RepositoryResult<TokenDto>(200, generatedToken, null);
            }
            catch (Exception e)
            {
                return new RepositoryResult<TokenDto>(500, null, e.ToMessageResult());
            }
        }



        public async Task<RepositoryResult<TokenDto>> RegisterUser(SignUpDto userInfo)
        {
            try
            {
                var newUser = new User
                {
                    UserName = userInfo.UserName,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    Email = userInfo.Email,
                    PhoneNumber = userInfo.PhoneNumber
                };

                var identityResult = await _userManager.CreateAsync(newUser, userInfo.Password);
                if (identityResult.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);

                    var generatedToken = await GenerateToken(newUser);
                    if (generatedToken == null)
                        return new RepositoryResult<TokenDto>(401, null, "Access denied!");
                    else
                        return new RepositoryResult<TokenDto>(200, generatedToken, "The operation was successful");
                }
                else
                {
                    var message = "One or more errors have occurred!";
                    if (identityResult.Errors.Any())
                    {
                        var error = identityResult.Errors.FirstOrDefault();
                        message = error.Code + " - Description : " + error.Description;
                    }
                    return new RepositoryResult<TokenDto>(500, null, message);
                }

            }
            catch (Exception e)
            {
                return new RepositoryResult<TokenDto>(500, null, e.ToMessageResult());
            }

        }

        private async Task<TokenDto> GenerateToken(User user)
        {


            try
            {
                var utcNow = DateTime.UtcNow;

                var claims = new List<Claim>
            {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        //new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        //new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                        //new Claim(JwtRegisteredClaimNames.Exp, utcNow.AddDays(_configuration.GetValue<int>("Tokens:Lifetime")).ToString())
            };

                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var signingKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Tokens:Key")));

                var signingCredentials =
                    new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var defaultLifeTime = 60;
                var lifeTime =
                    (_configuration.GetValue<string>("Tokens:LifeTime") ??
                    defaultLifeTime.ToString())
                    .SafeToInt(defaultLifeTime);

                var jwt = new JwtSecurityToken(
                    signingCredentials: signingCredentials,
                    claims: claims,
                    //notBefore: utcNow,
                    expires: utcNow.AddMinutes(lifeTime),
                    audience: _configuration.GetValue<string>("Tokens:Audience"),
                    issuer: _configuration.GetValue<string>("Tokens:Issuer")
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new TokenDto
                {
                    Token = token,
                    UserName = user.UserName,
                    UserRoles = userRoles.ToArray(),
                    LifeMinutes = lifeTime
                };
            }
            catch (Exception e)
            {
                return null;
            }

        }


    }
}
