using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CROPS.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CROPS.Accounts.DTOs
{
    [AutoMap(typeof(Account))]
    public class AccountDto : EntityDto
    {
        public int AccountId { get; set; }

        public string AccountName { get; set; }

        public int? ParentId { get; set; }

        public string Descriptions { get; set; }
    }
}
