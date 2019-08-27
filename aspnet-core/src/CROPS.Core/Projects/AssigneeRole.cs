using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace CROPS.Projects
{
    public class AssigneeRole : Entity
    {
        public string Role { get; set; }

        public int? RoleCode { get; set; }
    }
}
