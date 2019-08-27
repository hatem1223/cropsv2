using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CROPS.ALM
{
    public class Bug : IEntity, IWorkItem
    {
        [Key]
        public int BugId { get; set; }

        [Column("BugSourceId")]
        public int? SourceId { get; set; }

        public int? ProjectId { get; set; }

        public int? IterationSk { get; set; }

        public DateTime? CreationDate { get; set; }

        public string BugName { get; set; }

        public string State { get; set; }

        public int? IsActive { get; set; }

        public string AssignedTo { get; set; }

        public decimal? CompletedWork { get; set; }

        public decimal? OriginalEstimate { get; set; }

        public decimal? RemainingWork { get; set; }

        public string Activity { get; set; }

        public int? UserStorySourceId { get; set; }

        [NotMapped]
        public int Id { get => BugId; set => BugId = Id; }

        public void CopyFrom(IWorkItem workItem)
        {
            Bug bug = workItem as Bug;
            IterationSk = bug.IterationSk;
            ProjectId = bug.ProjectId;
            IsActive = bug.IsActive;
            BugName = bug.BugName;
            State = bug.State;
        }

        public bool IsTransient()
        {
            return Convert.ToInt32(Id) <= 0;
        }
    }
}
