using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CROPS.ALM
{
    public class Feature : IEntity, IWorkItem
    {
        public string State { get; set; }

        public int? IsActive { get; set; }

        public int FeatureId { get; set; }

        [Column("FeatureSourceId")]
        public int? SourceId { get; set; }

        public int? ProjectId { get; set; }

        public int? IterationSk { get; set; }

        public string FeatureName { get; set; }

        public DateTime? CreationDate { get; set; }

        [NotMapped]
        public int Id { get => FeatureId; set => FeatureId = Id; }

        public void CopyFrom(IWorkItem workItem)
        {
            Feature feature = workItem as Feature;
            FeatureName = feature.FeatureName;
            IterationSk = feature.IterationSk;
            ProjectId = feature.ProjectId;
            IsActive = feature.IsActive;
            State = feature.State;
        }

        public bool IsTransient()
        {
            return Convert.ToInt32(Id) <= 0;
        }
    }
}
