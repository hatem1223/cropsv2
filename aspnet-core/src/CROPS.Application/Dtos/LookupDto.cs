using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace CROPS.Dtos
{
    public class LookupDto<TPrimaryKey> : EntityDto<TPrimaryKey>
    {
        public string Name { get; set; }
    }
}
