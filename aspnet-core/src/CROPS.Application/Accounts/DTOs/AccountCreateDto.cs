using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CROPS.Projects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CROPS.Accounts.DTOs
{
    [AutoMap(typeof(Account))]
    public class AccountCreateDto : EntityDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "AccountName is required")]
        public string AccountName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        public string Descriptions { get; set; }
        
    }
}
