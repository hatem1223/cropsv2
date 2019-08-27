using System;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace CROPS.Projects
{
    public class AssigneeLevel : Entity
    {
        public string Level { get; set; }

        public int? LevelCode { get; set; }
    }
}
