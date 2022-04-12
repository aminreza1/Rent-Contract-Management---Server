using Microsoft.AspNetCore.Identity;
using MyRentalWebService.Data.Interfaces;
using MyRentalWebService.Data.Providers;
using MyRentalWebService.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Repository
{
    public class SetupRepository : ISetupRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _db;
        public SetupRepository(
            UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = context;
        }

        public async Task<bool> ConfigAdminUser()
        {
            try
            {
                var roleOp = new Role
                {
                    Name = "operator",
                    NormalizedName = "operator".ToUpper()
                };

                var checkRoleOp = await _roleManager.RoleExistsAsync(roleOp.Name);
                if (!checkRoleOp)
                    await _roleManager.CreateAsync(roleOp);

                var role = new Role
                {
                    Name = "admin",
                    NormalizedName = "admin".ToUpper()
                };

                var checkRole = await _roleManager.RoleExistsAsync(role.Name);
                if (!checkRole)
                    await _roleManager.CreateAsync(role);

                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@site.com",
                    PhoneNumber = "09130001234",
                    FirstName = "system",
                    LastName = "manager",
                    IsBlock = false
                };
                var checkUser = await _userManager.FindByNameAsync(user.UserName);
                if (checkUser == null)
                {
                    IdentityResult identityResult = await _userManager.CreateAsync(user, "Aa@123456");
                    IdentityResult result = identityResult;
                }
                else
                    user = checkUser;

                var checkUserHasRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (!checkUserHasRole)
                    await _userManager.AddToRoleAsync(user, role.Name);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> ConfigOptions()
        {
            try
            {
                var optionList = new List<Option>
                {
                    new Option{
                        Key = "themes",
                        Value = "dark,light"},
                    new Option{
                        Key = "use-theme",
                        Value = "dark"},
                    new Option{
                        Key = "languages",
                        Value = "en,fa"},
                    new Option{
                        Key = "use-language",
                        Value = "en"},
                    new Option{
                        Key = "currencies",
                        Value = "usd,gbp,eur,irr"},
                    new Option{
                        Key = "use-currency",
                        Value = "usd"}
                };

                foreach (var optionItem in optionList)
                {
                    var opt = _db.Options.FirstOrDefault(x => x.Key == optionItem.Key);
                    if (opt != null) continue;

                    _db.Options.Add(optionItem);
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
