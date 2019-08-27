using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CROPS.Reports
{
    public class Workspace : IEntity<string>
    {
        [Key]
        public string WorkspaceId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ICollection<Report> Reports { get; set; }

        public ICollection<Dashboard> Dashboards { get; set; }

        [NotMapped]
        public string Id { get => this.WorkspaceId; set => this.WorkspaceId = this.Id; }

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