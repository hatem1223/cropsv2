using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CROPS.Projects
{
    public class Iteration : IEntity
    {
        public int IterationId { get; set; }

        public int? IterationSourceId { get; set; }

        public int? ProjectId { get; set; }

        public string IterationName { get; set; }

        public int? Depth { get; set; }

        public DateTime? LastUpdatedDateTime { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Scope { get; set; }

        [NotMapped]
        public int Id { get => this.IterationId; set => this.IterationId = this.Id; }

        public bool IsTransient()
        {
            return Convert.ToInt32(this.Id) <= 0;
        }
    }
}
