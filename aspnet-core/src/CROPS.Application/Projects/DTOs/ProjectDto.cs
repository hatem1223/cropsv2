using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using CROPS.Projects.Enums;

namespace CROPS.Projects.DTOs
{
    [AutoMap(typeof(Project))]
    public class ProjectDto : EntityDto, ICustomValidate
    {
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(64)]
        public string ProjectName { get; set; }

        [Required]
        public int AccountId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        [Url]
        public string OnlineTfsCollectionUri { get; set; }

        public string CollectionName { get; set; }

        public string ReleaseName { get; set; }

        public bool IsOnPremProject { get; set; }

        public int? AuthType { get; set; }

        [Required]
        public string WorkspaceName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (IsOnPremProject == false)
            {
                if (string.IsNullOrEmpty(UserName))
                {
                    context.Results.Add(new ValidationResult("User name is required!"));
                }

                if (string.IsNullOrEmpty(Password))
                {
                    context.Results.Add(new ValidationResult("Password is required!"));
                }

                if (AuthType != (int)AuthenticationTypeEnum.AlternativePassword && AuthType != (int)AuthenticationTypeEnum.PersonalAccessToken)
                {
                    context.Results.Add(new ValidationResult("AuthType is required!"));
                }

                if (FromDate == null || FromDate == default(DateTime))
                {
                    context.Results.Add(new ValidationResult("From date is required!"));
                }

                if (FromDate != null && FromDate != default(DateTime)
                    && ToDate != null && ToDate != default(DateTime)
                    && FromDate > ToDate)
                {
                    context.Results.Add(new ValidationResult("From date must be smaller than to date!"));
                }
            }
        }
    }
}
