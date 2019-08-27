using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CROPS.ALM
{
    public class Task : IEntity, IWorkItem
    {
        public int TaskId { get; set; }

        [Column("TaskSourceId")]
        public int? SourceId { get; set; }

        public int? ProjectId { get; set; }

        public int? IterationSk { get; set; }

        public DateTime? CreationDate { get; set; }

        public string TaskName { get; set; }

        public string State { get; set; }

        public int? IsActive { get; set; }

        public string AssignedTo { get; set; }

        public decimal? CompletedWork { get; set; }

        public decimal? OriginalEstimate { get; set; }

        public decimal? RemainingWork { get; set; }

        public string Activity { get; set; }

        public int? UserStorySourceId { get; set; }

        [NotMapped]
        public int Id { get => TaskId; set => TaskId = Id; }

        public void CopyFrom(IWorkItem workItem)
        {
            Task task = workItem as Task;
            IterationSk = task.IterationSk;
            ProjectId = task.ProjectId;
            TaskName = task.TaskName;
            IsActive = task.IsActive;
            State = task.State;
        }

        public bool IsTransient()
        {
            return Convert.ToInt32(Id) <= 0;
        }
    }
}
