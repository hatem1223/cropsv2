using System;
using Abp.Application.Services.Dto;

namespace CROPS.Users.DTOs
{
    public class RoleDTO : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}
