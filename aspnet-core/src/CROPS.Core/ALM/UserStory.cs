using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace CROPS.ALM
{
    public class UserStory : IEntity, IWorkItem
    {
        public int UserStoryId { get; set; }

        [Column("UserStorySourceId")]
        public int? SourceId { get; set; }

        public int? ProjectId { get; set; }

        public int? IterationSk { get; set; }

        public DateTime? CreationDate { get; set; }

        public string UserStoryName { get; set; }

        public string State { get; set; }

        public string AssignedTo { get; set; }

        public int? StoryPoints { get; set; }

        public int? IsActive { get; set; }

        public bool? IsCr { get; set; }

        public int? FeatureSourceId { get; set; }

        [NotMapped]
        public int Id { get => UserStoryId; set => UserStoryId = Id; }

        public void CopyFrom(IWorkItem workItem)
        {
            UserStory userStory = workItem as UserStory;
            UserStoryName = userStory.UserStoryName;
            IterationSk = userStory.IterationSk;
            ProjectId = userStory.ProjectId;
            IsActive = userStory.IsActive;
            State = userStory.State;
        }

        public bool IsTransient()
        {
            return Convert.ToInt32(Id) <= 0;
        }
    }
}
