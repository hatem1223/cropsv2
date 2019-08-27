using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CROPS.ALM
{
    public class Epic : IEntity, IWorkItem
    {
        public int EpicId { get; set; }

        public int? ProjectId { get; set; }

        public int? IterationSk { get; set; }

        public DateTime? CreationDate { get; set; }

        public string EpicName { get; set; }

        public string State { get; set; }

        public int? IsActive { get; set; }

        [NotMapped]
        public int Id { get => EpicId; set => EpicId = Id; }

        [Column("EpicSourceId")]
        public int? SourceId { get; set; }

        public void CopyFrom(IWorkItem workItem)
        {
            Epic epic = workItem as Epic;
            EpicName = epic.EpicName;
            IsActive = epic.IsActive;
            IterationSk = epic.IterationSk;
            ProjectId = epic.ProjectId;
            State = epic.State;
        }

        public bool IsTransient()
        {
            return Convert.ToInt32(Id) <= 0;
        }
    }
}
