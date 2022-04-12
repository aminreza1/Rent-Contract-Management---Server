using Microsoft.AspNetCore.Identity;
using MyRentalWebService.Data.Dtos;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Data.Providers;
using MyRentalWebService.Infrastructure.ExtensionMethods;
using MyRentalWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _db;
        public UserRepository(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = context;
        }

        public async Task<RepositoryResult<PaginatorDto<UserDto>>> GetAllUsers(int pageIndex, int pageSize)
        {
            var length = _userManager.Users.Count();

            var query = _userManager.Users
                .OrderByDescending(o => o.CreateDateTime)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            var items = new List<UserDto>();
            foreach (var user in query.ToList())
            {
                var item = new UserDto
                {
                    Id = user.Id,
                    CreateDateTime = user.CreateDateTime.ToString("u"),
                    CreateDateTimeAsDate = user.CreateDateTime,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Mobile = user.PhoneNumber,
                    UserName = user.UserName,
                    IsBlock = user.IsBlock
                };
                var roles = (await _userManager.GetRolesAsync(user)).ToList();
                item.Roles = roles;
                items.Add(item);
            }

            var data = new PaginatorDto<UserDto>
            {
                Length = length,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = items
            };
            return new RepositoryResult<PaginatorDto<UserDto>>(200, data, null);
        }
        public RepositoryResult<IEnumerable<RolesInUserOperationDto>> GetAllRoles()
        {
            var data = _roleManager.Roles.Select(role => new RolesInUserOperationDto
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();
            return new RepositoryResult<IEnumerable<RolesInUserOperationDto>>(200, data, null);
        }
        public async Task<RepositoryResult> CreateUser(CreateUserDto user)
        {
            try
            {
                if (
                    string.IsNullOrWhiteSpace(user.UserName) ||
                    string.IsNullOrWhiteSpace(user.Password) ||
                    string.IsNullOrWhiteSpace(user.Mobile) ||
                    user.Roles.Count < 1)
                    return new RepositoryResult(404, "You must complete the required fields");

                var newUser = new User
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.Mobile,
                    CreateDateTime = DateTime.Now,
                    EmailConfirmed = user.EmailConfirmed,
                    IsBlock = user.IsBlock,
                    PhoneNumberConfirmed = user.MobileConfirmed
                };

                var identityResult = await _userManager.CreateAsync(newUser, user.Password);
                if (identityResult.Succeeded)
                {
                    foreach (var role in user.Roles)
                    {
                        await _userManager.AddToRoleAsync(newUser, role);
                    }
                    return new RepositoryResult(200, "The operation was successful");
                }
                else
                {
                    var message = "One or more errors have occurred!";
                    if (identityResult.Errors.Any())
                    {
                        var error = identityResult.Errors.FirstOrDefault();
                        message = error.Code + " - Description : " + error.Description;
                    }
                    return new RepositoryResult(500, message);
                }
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }
        public async Task<RepositoryResult<UpdateUserDto>> GetUserByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return new RepositoryResult<UpdateUserDto>(404, null, "User not found!");

            var data = new UpdateUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Mobile = user.PhoneNumber,
                MobileConfirmed = user.PhoneNumberConfirmed,
                EmailConfirmed = user.EmailConfirmed,
                IsBlock = user.IsBlock,
                ChangeForcePassword = false
            };
            var roles = (await _userManager.GetRolesAsync(user)).ToList();
            data.Roles = roles;

            return new RepositoryResult<UpdateUserDto>(200, data, "");
        }

        public async Task<RepositoryResult> UpdateUser(UpdateUserDto user)
        {
            try
            {
                if (
                   string.IsNullOrWhiteSpace(user.UserName) ||
                   string.IsNullOrWhiteSpace(user.Mobile) ||
                   user.Roles.Count < 1)
                    return new RepositoryResult(400, "You must complete the required fields");

                var dbUser = await _userManager.FindByNameAsync(user.UserName);
                if (dbUser == null)
                    return new RepositoryResult(404, "User not found!");

                // Update user info
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Email = user.Email;
                dbUser.EmailConfirmed = user.EmailConfirmed;
                dbUser.PhoneNumber = user.Mobile;
                dbUser.PhoneNumberConfirmed = user.MobileConfirmed;
                dbUser.IsBlock = user.IsBlock;

                var updateResult = await _userManager.UpdateAsync(dbUser);
                if (!updateResult.Succeeded)
                    return new RepositoryResult(500, "The operation was failed.");

                // Force Change Password 
                if (user.ChangeForcePassword)
                {
                    if (string.IsNullOrWhiteSpace(user.Password))
                        return new RepositoryResult(400, "You must complete the required fields");

                    var removePassRes = await _userManager.RemovePasswordAsync(dbUser);
                    if (!removePassRes.Succeeded)
                        return new RepositoryResult(500, "Change password was failed.");
                    var addPassRes = await _userManager.AddPasswordAsync(dbUser, user.Password);
                    if (!addPassRes.Succeeded)
                        return new RepositoryResult(500, "Change password was failed.");
                }

                // Update Roles 
                var dbUserRoles = await _userManager.GetRolesAsync(dbUser);
                var removeRolesRes = await _userManager.RemoveFromRolesAsync(dbUser, dbUserRoles);
                if (!removeRolesRes.Succeeded)
                    return new RepositoryResult(500, "Update roles was failed.");
                var addRolesRes = await _userManager.AddToRolesAsync(dbUser, user.Roles);
                if (!removeRolesRes.Succeeded)
                    return new RepositoryResult(500, "Update roles was failed.");

                return new RepositoryResult(204, "Operations was successfull.");
            }
            catch (Exception e)
            {
                return new RepositoryResult(500, e.ToMessageResult());
            }
        }
    }
}
