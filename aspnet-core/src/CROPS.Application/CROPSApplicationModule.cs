using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using CROPS.HttpClientUtility;
using CROPS.Projects;
using CROPS.Projects.DTOs;
using System.Net.Http;

namespace CROPS
{
    [DependsOn(
        typeof(CROPSCoreModule),
        typeof(AbpAutoMapperModule))]
    public class CROPSApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            var thisAssembly = typeof(CROPSApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);
            IocManager.Register<IHttpClientManager, HttpClientManager>(DependencyLifeStyle.Transient);
            IocManager.Register<HttpClient, HttpClient>(DependencyLifeStyle.Transient);
            // Scan the assembly for classes which inherit from AutoMapper.Profile
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                cfg => cfg.AddProfiles(thisAssembly));
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                config.CreateMap<ProjectDetail, ProjectDetailDto>()
                      .ForMember(u => u.OptimisticIteration, options => options.MapFrom(input => input.OptimisticFinalIteration))
                      .ForMember(u => u.PessimisticIteration, options => options.MapFrom(input => input.PessimisticFinalIteration)).ReverseMap();

                config.CreateMap<ProjectDto, Project>()
                      .ForMember(u => u.ProjectAreaPath, options => options.MapFrom(input => input.IsOnPremProject ? $"\\{input.CollectionName}\\{input.ReleaseName}" : input.OnlineTfsCollectionUri));

                config.CreateMap<Project, ProjectDto>()
                      .ForMember(u => u.ReleaseName, options => options.Ignore())
                      .ForMember(u => u.CollectionName, options => options.Ignore())
                      .ForMember(u => u.OnlineTfsCollectionUri, options => options.Ignore());
            });
        }
    }
}
