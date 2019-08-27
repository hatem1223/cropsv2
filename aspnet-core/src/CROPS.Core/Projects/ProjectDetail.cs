using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities;
using CROPS.Contracts;
using CROPS.MappingAttributes;

namespace CROPS.Projects
{
    public class ProjectDetail : IEntity, IValidatableObject, IHasName
    {
        [Key]
        public int ProjectDetailsId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        public int ProjectId { get; set; }

        public int OptimisticFinalIteration { get; set; }

        public int PessimisticFinalIteration { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? Scope { get; set; }

        /// <summary>
        /// Gets or Sets Relative url pointinig to Content folder in web
        /// Projects/{projectId}.ext
        /// </summary>
        public string Logo { get; set; }

        public string Descriptions { get; set; }

        public DateTime PullDataFromDate { get; set; }

        public DateTime PullDataToDate { get; set; }

        [NotMapped]
        public int Id { get => this.ProjectDetailsId; set => this.ProjectDetailsId = this.Id; }

        public string DisplayName => Descriptions;

        public bool IsTransient()
        {
            return Convert.ToInt32(this.Id) <= 0;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }
    }
}
