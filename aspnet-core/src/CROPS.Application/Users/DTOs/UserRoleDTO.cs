using System;
using Abp.Application.Services.Dto;

namespace CROPS.Users.DTOs
{
    public class UserRoleDTO : EntityDto<Guid>
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RoleName { get; set; }
    }
}
