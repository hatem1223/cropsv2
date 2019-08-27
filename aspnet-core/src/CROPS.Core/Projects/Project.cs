using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using CROPS.Contracts;

namespace CROPS.Projects
{
    public class Project : IEntity, IHasName
    {
        public int ProjectId { get; set; }

        public int? ParentSourceId { get; set; }

        public int? ProjectSourceId { get; set; }

        public int? ReleaseSourceId { get; set; }

        public string ProjectName { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsActive { get; set; }

        public string ProjectAreaPath { get; set; }

        public int AccountId { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public string ProjectType { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int? AuthType { get; set; }

        public string WorkspaceId { get; set; }

        [NotMapped]
        public int Id { get => this.ProjectId; set => this.ProjectId = this.Id; }

        public string DisplayName
        {
            get { return ProjectName; }
        }

        public bool IsTransient()
        {
            return Convert.ToInt32(this.Id) <= 0;
        }
    }
}
