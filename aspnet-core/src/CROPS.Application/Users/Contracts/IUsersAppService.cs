using Abp.Application.Services;
using CROPS.Dtos;
using CROPS.Users.DTOs;
using System;

namespace CROPS.Users.Contracts
{
    public interface IUsersAppService : IAsyncCrudAppService<UserRoleDTO, Guid, FilteredResultRequestDto>
    {
    }
}
