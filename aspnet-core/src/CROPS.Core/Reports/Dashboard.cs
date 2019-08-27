using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;

namespace CROPS.Reports
{
    public class Dashboard : IEntity<string>
    {
        [Key]
        public string DashboardId { get; set; }

        public string Name { get; set; }

        public virtual Workspace Workspace { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [StringLength(1500)]
        public string Description { get; set; }

        public string WorkspaceId { get; set; }

        [NotMapped]
        public string Id { get => this.DashboardId; set => this.DashboardId = this.Id; }

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
