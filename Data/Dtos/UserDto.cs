using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRentalWebService.Data.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CreateDateTime { get; set; }
        public DateTime CreateDateTimeAsDate { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public bool IsBlock { get; set; }
        public List<string> Roles { get; set; }

    }
    public class RolesInUserOperationDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class CreateUserDto
    {
        public CreateUserDto()
        {
            Roles = new List<string>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public bool MobileConfirmed { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBlock { get; set; }
        public List<string> Roles { get; set; }

    }
    public class UpdateUserDto
    {
        public UpdateUserDto()
        {
            Roles = new List<string>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ChangeForcePassword { get; set; }
        public string Mobile { get; set; }
        public bool MobileConfirmed { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsBlock { get; set; }
        public List<string> Roles { get; set; }

    }
}
