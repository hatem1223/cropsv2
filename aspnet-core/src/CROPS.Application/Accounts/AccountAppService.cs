using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Modules;
using Abp.ObjectMapping;
using Abp.UI;
using CROPS.Accounts.Contracts;
using CROPS.Accounts.DTOs;
using CROPS.Dtos;
using CROPS.Projects;
using CROPS.Projects.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CROPS.Accounts
{
    public class AccountAppService : FilteredCrudAppService<Account, AccountDto, int, FilteredResultRequestDto, AccountCreateDto>, IAccountAppService
    {
        private IRepository<Account> repo;
        public AccountAppService(IRepository<Account> repository)
            : base(repository)
        {
            repo = repository;
        }
       
        public override async Task<AccountDto> Create(AccountCreateDto input)
        {

            AccountDto resultdto = null;
            var message = string.Empty;

            // check for duplicates
            var found = repo.FirstOrDefault(x => x.AccountName == input.AccountName);
            if (found == null)
            {
                return await base.Create(input);
            }
            else
            {
                message = "Account already exists.";
                throw new UserFriendlyException(message);
            }
        }
    }
}
