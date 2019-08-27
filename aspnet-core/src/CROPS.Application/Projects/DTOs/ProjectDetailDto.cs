using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using CROPS.MappingAttributes;

namespace CROPS.Projects.DTOs
{
    [AutoMap(typeof(ProjectDetail))]
    public class ProjectDetailDto : EntityDto,  ICustomValidate
    {
        public const string PessimisticOptemisticValidationMsg = "PessimisticIteration shouldn't be greate than OptimisticIteration";
        public const string LessThanOneValidationMsg = "Please enter a value bigger than 1";
        public const string MaximumDescriptionValidationMsg = "Description should be 500 characters Maximum";
        public const string PullFromDateGTPullToDateValidationMsg = "PullDataTo date must be prior to PullDataFrom date";

        [Required]
        public int ProjectId { get; set; }

        [Mapping("OptimisticFinalIteration")]
        [Range(1, int.MaxValue, ErrorMessage = LessThanOneValidationMsg)]
        public int OptimisticIteration { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = LessThanOneValidationMsg)]
        [Mapping("PessimisticFinalIteration")]
        public int PessimisticIteration { get; set; }

        [Required]
        public DateTime? CreationDate { get; set; }

        [Required]
        public int? Scope { get; set; }

        /// <summary>
        /// Gets or Sets Relative url pointinig to Content folder in web
        /// Projects/{projectId}.ext
        /// </summary>
        public string Logo { get; set; }

        [MaxLength(500, ErrorMessage = MaximumDescriptionValidationMsg)]
        public string Descriptions { get; set; }

        public DateTime PullDataFromDate { get; set; }

        public DateTime PullDataToDate { get; set; }

        public void AddValidationErrors(CustomValidationContext context)
        {
            if (PessimisticIteration > OptimisticIteration)
            {
                context.Results.Add(new ValidationResult(PessimisticOptemisticValidationMsg));
            }

            if (PullDataFromDate > PullDataToDate)
            {
                context.Results.Add(new ValidationResult(PullFromDateGTPullToDateValidationMsg));
            }
        }
    }
}
