using Abp.Application.Services.Dto;

namespace CROPS.Dtos
{
    public class FilteredResultRequestDto : PagedAndSortedResultRequestDto, IFilterResultRequest
    {
        /// <summary>
        /// Gets or Sets filter option in OData v4 query options format, example: Id eq 2 or Name eq 'name3'
        /// </summary>
        public string Filter { get; set; }
    }
}
