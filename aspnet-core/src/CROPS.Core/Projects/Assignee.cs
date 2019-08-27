using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace CROPS.Projects
{
    public class Assignee : Entity
    {
        public int? ProjectId { get; set; }

        public string Name { get; set; }

        public int? RoleCode { get; set; }

        public int? LevelCode { get; set; }

        public int? IsActive { get; set; }
    }
}
