using Abp.Application.Services;
using CROPS.Accounts.DTOs;
using CROPS.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CROPS.Accounts.Contracts
{
    public interface IAccountAppService : IAsyncCrudAppService<AccountDto, int, FilteredResultRequestDto, AccountCreateDto>
    {
    }
}
