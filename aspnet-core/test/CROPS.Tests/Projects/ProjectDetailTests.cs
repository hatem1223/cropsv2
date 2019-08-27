using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using CROPS.Projects;
using CROPS.Projects.Contracts;
using CROPS.Projects.DTOs;
using Shouldly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CROPS.Tests.Projects
{

    public class ProjectDetailTests : CROPSTestBase
    {

        private readonly IProjectDetailsAppService projectDetailsAppService;

        public ProjectDetailTests()
        {
            projectDetailsAppService = Resolve<IProjectDetailsAppService>();

            // Arrange
            UsingDbContext(context =>
            {
                // var data = A.ListOf<ProjectDetail>(5);
                var data = new List<ProjectDetail>
                {
                    new ProjectDetail { Id = 1, ProjectId = 1, Scope = 1, OptimisticFinalIteration = 2, PessimisticFinalIteration = 2 },
                    new ProjectDetail { Id = 2, ProjectId = 2, Scope = 2, OptimisticFinalIteration = 2, PessimisticFinalIteration = 2 },
                    new ProjectDetail { Id = 3, ProjectId = 3, Scope = 2, OptimisticFinalIteration = 3, PessimisticFinalIteration = 3 },
                    new ProjectDetail { Id = 4, ProjectId = 4, Scope = 3, OptimisticFinalIteration = 3, PessimisticFinalIteration = 3 },
                    new ProjectDetail { Id = 5, ProjectId = 5, Scope = 4, OptimisticFinalIteration = 4, PessimisticFinalIteration = 4 }
                };

               
                context.ProjectDetails.AddRange(data);
            });
        }
        #region GetAll

        [Fact]
        public async Task GetAll_NoFilter_ShouldReturnAllItems()
        {
            // Act
            var result = await projectDetailsAppService.GetAll(new Dtos.FilteredResultRequestDto());

            // Assert
            result.Items.Count.ShouldBe(5);
            result.ShouldBeOfType<PagedResultDto<ProjectDetailDto>>().ShouldNotBeNull();
        }

        [Theory]
        [InlineData("projectId eq 1", 1)]
        [InlineData("projectId eq 2", 1)]
        [InlineData("scope eq 2", 2)]
        [InlineData("OptimisticIteration eq 2", 2)]
        [InlineData("PessimisticIteration eq 2", 2)]
        [InlineData("PessimisticIteration eq 2 and Scope eq 1", 1)]
        [InlineData("PessimisticIteration eq 2 or Scope eq 1", 2)]
        [InlineData("PessimisticIteration eq 2 and Scope gt 1", 1)]
        public async Task GetAll_WithFilter_ShouldReturnAllItemsFiltered(string filter, int expectedResultCount)
        {
            // Act
            var result = await projectDetailsAppService.GetAll(new Dtos.FilteredResultRequestDto() { Filter = filter });

            // Assert
            result.Items.Count.ShouldBe(expectedResultCount);
            result.ShouldBeOfType<PagedResultDto<ProjectDetailDto>>().ShouldNotBeNull();
        }
        #endregion

        #region Get

        [Theory]
        [InlineData(1)]
        [InlineData(2)]

        public async Task Get_ValidProjectDetail_ShouldReturnsProjectDetail(int id)
        {
            // Act
            var projectDetail = await projectDetailsAppService.Get(new EntityDto<int> { Id = id }).ConfigureAwait(false);

            // Assert
            projectDetail.ShouldBeOfType<ProjectDetailDto>().ShouldNotBeNull();
            projectDetail.ProjectId.ShouldBe(id);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(21)]
        public async Task Get_InvalidProjectDetail_ShouldThrowEntityNotFoundException(int id)
        {
            Should.Throw<EntityNotFoundException>(async () => await projectDetailsAppService.Get(new EntityDto<int> { Id = id }).ConfigureAwait(false));
        }
        #endregion

        #region Create

        [Fact]
        public async Task Create_ValidProjectDetail_ShouldBeAdded()
        {
            //// Arrange
            //var projectDetail = new ProjectDetailDto
            //{
            //    Id = 6,
            //    ProjectId = 1,
            //    Scope = 10,
            //    OptimisticIteration = 23,
            //    PessimisticIteration = 23
            //};

            //// Act
            //var projectDetailDto = await projectDetailsAppService.Create(projectDetail);

            //// Assert
            //projectDetailDto.ShouldBeOfType<ProjectDetailDto>().ShouldNotBeNull();
            //projectDetailDto.ProjectId.ShouldBe<int>(6);
        }

        // [Fact]
        // public async Task Create_NotValidProjectDetail_ShouldNotBeAdded()
        // {
        //    // Arrange
        //    var projectDetail = new ProjectDetailDto
        //    {
        //        ProjectId = 1,
        //        Scope = 7,
        //        OptimisticIteration = 7,
        //        PessimisticIteration = 7
        //    };
        //    Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false));
        // }

        [Fact]
        public async Task Create_LessThanOneValuesProjectDetail_ShouldThrowValidationException()
        {
            // Arrange
            var projectDetail = new ProjectDetailDto
            {
                Id = 10,
                ProjectId = 20,
                Scope = 7,
                OptimisticIteration = -2,
                PessimisticIteration = -2
            };
            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.LessThanOneValidationMsg);
        }

        [Fact]
        public async Task Create_PestimistigGTOptemistic_ShouldThrowValidationException()
        {
            // Arrange
            var projectDetail = new ProjectDetailDto
            {
                Id = 10,
                ProjectId = 20,
                Scope = 7,
                OptimisticIteration = 2,
                PessimisticIteration = 5
            };
            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.PessimisticOptemisticValidationMsg);
        }

        [Fact]
        public async Task Create_DescriptionMoreThanMaximumProjectDetail_ShouldThrowValidationException()
        {
            // Arrange
            var projectDetail = new ProjectDetailDto
            {
                Id = 10,
                ProjectId = 20,
                OptimisticIteration = 5,
                PessimisticIteration = 5,
                Descriptions = new string('*', 501)
            };
            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.MaximumDescriptionValidationMsg);
        }

        [Fact]
        public async Task Create_PullFromDateGTPullToDateProjectDetail_ShouldThrowValidationException()
        {
            // Arrange
            var projectDetail = new ProjectDetailDto
            {
                Id = 10,
                ProjectId = 20,
                OptimisticIteration = 5,
                PessimisticIteration = 5,
                PullDataFromDate = DateTime.Now,
                PullDataToDate = DateTime.Now.AddDays(-1)
            };
            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.PullFromDateGTPullToDateValidationMsg);
        }
        #endregion

        #region Update
        [Fact]
        public async Task Update_ValidProjectDetail_ShouldBeUpdated()
        {
            // Arrange
            //var projectDetail = new ProjectDetailDto
            //{
            //    Id = 1,
            //    ProjectId = 1,
            //    Scope = 20,
            //    OptimisticIteration = 20,
            //    PessimisticIteration = 20,
            //    Descriptions = "New Description"
            //};
            //var updatedProjectDetail = projectDetailsAppService.Update(projectDetail).Result;
            //updatedProjectDetail.ShouldBeOfType<ProjectDetailDto>().Scope.ShouldBe(20);
            //updatedProjectDetail.Descriptions.ShouldBe("New Description");
        }

        [Fact]
        public async Task Update_NotValidProjectDetail_ShouldThrowEntityNotFoundException()
        {
            //var projectDetail = await projectDetailsAppService.Get(new EntityDto<int> { Id = 1 }).ConfigureAwait(false);
            //projectDetail.Id = 100;

            //Should.Throw<EntityNotFoundException>(async () => await projectDetailsAppService.Update(projectDetail).ConfigureAwait(false));
        }

        public async Task Update_LessThanOneValuesProjectDetail_ShouldThrowValidationException()
        {
            var projectDetail = await projectDetailsAppService.Get(new EntityDto<int> { Id = 1 }).ConfigureAwait(false);
            projectDetail.OptimisticIteration = -2;

            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.LessThanOneValidationMsg);
        }

        [Fact]
        public async Task Update_PestimistigGTOptemistic_ShouldThrowValidationException()
        {
            var projectDetail = await projectDetailsAppService.Get(new EntityDto<int> { Id = 1 }).ConfigureAwait(false);
            projectDetail.OptimisticIteration = 2;
            projectDetail.PessimisticIteration = 5;

            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.PessimisticOptemisticValidationMsg);
        }

        [Fact]
        public async Task Update_DescriptionMoreThanMaximumProjectDetail_ShouldThrowValidationException()
        {
            var projectDetail = await projectDetailsAppService.Get(new EntityDto<int> { Id = 1 }).ConfigureAwait(false);
            projectDetail.Descriptions = new string('*', 501);

            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.MaximumDescriptionValidationMsg);
        }

        [Fact]
        public async Task Update_PullFromDateGTPullToDateProjectDetail_ShouldThrowValidationException()
        {
            var projectDetail = await projectDetailsAppService.Get(new EntityDto<int> { Id = 1 }).ConfigureAwait(false);
            projectDetail.PullDataFromDate = DateTime.Now;
            projectDetail.PullDataToDate = DateTime.Now.AddDays(-1);

            Should.Throw<AbpValidationException>(async () => await projectDetailsAppService.Create(projectDetail).ConfigureAwait(false))
            .ValidationErrors.ShouldContain(vr => vr.ErrorMessage == ProjectDetailDto.PullFromDateGTPullToDateValidationMsg);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_ValidProjectDetail_ShouldBeDeleted()
        {
            var countBefore = projectDetailsAppService.GetAll(new Dtos.FilteredResultRequestDto()).Result.TotalCount;
            await projectDetailsAppService.Delete(new EntityDto<int> { Id = 1 }).ConfigureAwait(false);
            var countAfter = projectDetailsAppService.GetAll(new Dtos.FilteredResultRequestDto()).Result.TotalCount;
            countBefore.ShouldBe(countAfter + 1);
        }

        [Fact]
        public async Task Delete_NotValidProjectDetail_ShouldThrowEntityNotFoundException()
        {
            Should.Throw<EntityNotFoundException>(async () => await projectDetailsAppService.Get(new EntityDto<int> { Id = 100 }).ConfigureAwait(false));
        }
        #endregion
    }
}
