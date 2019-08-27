using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Community.OData.Linq;
using CROPS.Contracts;
using CROPS.Dtos;
using CROPS.MappingAttributes;

namespace CROPS
{
    public abstract class FilteredCrudAppService<TEntity, TEntityDto>
        : FilteredCrudAppService<TEntity, TEntityDto, int>
        where TEntity : class, IEntity<int>
        where TEntityDto : IEntityDto<int>
    {
        protected FilteredCrudAppService(IRepository<TEntity, int> repository)
            : base(repository)
        {
        }
    }

    public abstract class FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey>
        : FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, FilteredResultRequestDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected FilteredCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
        }
    }

    public abstract class FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput>
        : FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
    {
        protected FilteredCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
        }
    }

    public abstract class FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput>
        : FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TCreateInput>
        where TGetAllInput : IPagedAndSortedResultRequest
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TCreateInput : IEntityDto<TPrimaryKey>
    {
        protected FilteredCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
        }
    }

    public abstract class FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput>
        : FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
    {
        protected FilteredCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
        }
    }

    public abstract class FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    : FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto<TPrimaryKey>>
        where TEntity : class, IEntity<TPrimaryKey>
        where TEntityDto : IEntityDto<TPrimaryKey>
        where TUpdateInput : IEntityDto<TPrimaryKey>
        where TGetInput : IEntityDto<TPrimaryKey>
    {
        protected FilteredCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
        }
    }

    public abstract class FilteredCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
       : AsyncCrudAppService<TEntity, TEntityDto, TPrimaryKey, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
           where TEntity : class, IEntity<TPrimaryKey>
           where TEntityDto : IEntityDto<TPrimaryKey>
           where TUpdateInput : IEntityDto<TPrimaryKey>
           where TGetInput : IEntityDto<TPrimaryKey>
           where TDeleteInput : IEntityDto<TPrimaryKey>
    {
        protected FilteredCrudAppService(IRepository<TEntity, TPrimaryKey> repository)
            : base(repository)
        {
            LocalizationSourceName = CROPSConsts.LocalizationSourceName;
        }

        //public override async Task<TEntityDto> Create(TCreateInput input)
        //{
        //    CheckCreatePermission();

        //    var entity = MapToEntity(input);

        //    await Repository.InsertAsync(entity);
        //    await CurrentUnitOfWork.SaveChangesAsync();

        //    return MapToEntityDto(entity);
        //}

        public override async Task<PagedResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            query = ApplyFiltering(query, input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);

            return new PagedResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        public virtual IList<LookupDto<TPrimaryKey>> GetLookup()
        {
            CheckGetAllPermission();
            return Repository.GetAll().Select(x =>
                new LookupDto<TPrimaryKey>()
                {
                    Id = x.Id,
                    Name = ((x as IHasName) != null) ? (x as IHasName).DisplayName : x.Id.ToString()
                }).OrderBy(p => p.Name).ToList<LookupDto<TPrimaryKey>>();
        }

        protected virtual IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, TGetAllInput input)
        {
            var filterInput = input as IFilterResultRequest;
            if (filterInput != null && !string.IsNullOrWhiteSpace(filterInput.Filter))
            {
                string MappedFilter = FilterMapping(filterInput.Filter);
                return query.OData().Filter(MappedFilter);
            }

            return query;
        }

        private string FilterMapping(string filter)
        {
            CultureInfo cultureInfo = new CultureInfo("en-US", false);
            filter = filter.ToLower(cultureInfo);
            // Validate Odata 
            // TODO
            // string strRegex = @"(?<Filter>" +
            // "\n" + @"     (?<Resource>.+?)\s+" +
            // "\n" + @"     (?<Operator>eq|ne|gt|ge|lt|le|add|sub|mul|div|mod)\s+" +
            // "\n" + @"     '?(?<Value>.+?)'?" +
            // "\n" + @")" +
            // "\n" + @"(?:" +
            // "\n" + @"    \s*$" +
            // "\n" + @"   |\s+(?:or|and|not)\s+" +
            // "\n" + @")" +
            // "\n";

            // Regex myRegex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            // string strReplace = @"${Resource}:${Value},";
            // var res = myRegex.Replace(filter, strReplace);
            // Dictionary<string, string> keyValuePairs = GetKeyValue(res.TrimEnd(','));

            // TRY2
            // string rejexStr = @"(?<PropertyName>\w+?)\s+(?<Operator>eq)\s+(?<Value>(['])(\\?.)*?\1|\d+(\.\d+)?(\s*|$))(?:\s*$|\s+(?:and)(\s+|$))";
            // string strReplace = @"${Filter} ${Resource} ${Operator} ${Value},";
            // Regex myRegex2 = new Regex(rejexStr, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
            foreach (var prop in typeof(TEntityDto).GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(MappingAttribute))))
            {
                var attrs = (MappingAttribute[])prop.GetCustomAttributes(typeof(MappingAttribute), false);
                foreach (var attr in attrs)
                {
                    string modelAttribute = attr.ModelName.ToLower(cultureInfo);
                    string dtoAttribute = prop.Name.ToLower(cultureInfo);
                    filter = ReplaceWholeMatch(filter, dtoAttribute, modelAttribute);

                    // filter = new string(filter.Replace(DtoAttribute, ModelAttribute));
                }
            }

            return filter;
        }

        private Dictionary<string, string> GetKeyValue(string input)
        {
            return input.Split(',').ToDictionary(kv => kv.Split(':').First(), kv => kv.Split(':').Last());
        }

        private string ReplaceWholeMatch(string input, string oldValue, string newValue)
        {
            string pattern = $"\\b{oldValue}\\b";
            return Regex.Replace(input, pattern, newValue);
        }

    }
}
