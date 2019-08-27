using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;

namespace CROPS.Reports
{
    public class Report : IEntity<string>
    {
        [Key]
        [StringLength(450)]
        public string ReportId { get; set; }

        public string Name { get; set; }

        [StringLength(450)]
        public string WorkspaceId { get; set; }

        public bool IsActive { get; set; }

        [StringLength(1500)]
        public string Description { get; set; }

        public virtual Workspace Workspace { get; set; }

        [NotMapped]
        public string Id { get => this.ReportId; set => this.ReportId = this.Id; }

        public bool IsTransient()
        {
            if (EqualityComparer<string>.Default.Equals(this.Id, default(string)))
            {
                return true;
            }

            return false;
        }
    }
}
