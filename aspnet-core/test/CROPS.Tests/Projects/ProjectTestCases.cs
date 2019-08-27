using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using CROPS.ALM;
using CROPS.Projects;
using CROPS.Projects.Contracts;
using CROPS.Projects.DTOs;
using CROPS.Projects.Enums;
using CROPS.Reports;
using GenFu;
using Microsoft.Extensions.Configuration;
using Microsoft.PowerBI.Api.V2.Models;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CROPS.Tests.Projects
{
    public class ProjectTestCases : CROPSTestBase
    {

        private readonly IProjectAppService projectAppService;
        private Mock<IAlmProvider> mockAlmProvider = new Mock<IAlmProvider>();
        private Mock<IReportsProvider> mockAIReportsProvider = new Mock<IReportsProvider>();
        private Mock<IConfiguration> mockAIConfig = new Mock<IConfiguration>();



        public ProjectTestCases()
        {
            RegisterInstance<IAlmProvider>(mockAlmProvider.Object);
            RegisterInstance<IReportsProvider>(mockAIReportsProvider.Object);
            RegisterInstance<IConfiguration>(mockAIConfig.Object);


            Project pro = new Project
            {
                Id = 1,
                ProjectId = 1,
                Password = "sdasdas",
                UserName = "sdawww",
                AuthType = (int)AuthenticationTypeEnum.AlternativePassword,
            };

      
            mockAIConfig.SetupGet(p => p["Configurations:reservedWords"]).Returns("PRN,COM1,COM2,COM3,COM4,COM5,COM6,COM7,COM8,COM9,COM10,LPT1,LPT2,LPT3,LPT4,LPT5,LPT6,LPT7,LPT8,LPT9,NUL,CON,AUX,SERVER,SignalR,DefaultCollection,Web,App_Browsers,App_code,App_Data,App_GlobalResources,App_LocalResources,App_Themes,App_WebResources,bin,web.config");
            mockAIConfig.SetupGet(p => p["Configurations:startsWithRestrictions"]).Returns(".,_");
            mockAIConfig.SetupGet(p => p["Configurations:endsWithRestrictions"]).Returns(".");
            mockAIConfig.SetupGet(p => p["Configurations:invalidSpecialCharacters"]).Returns("~='\"<>[]{}\\,&?/:;$%@#+*|");


            mockAlmProvider
                .Setup(e => e.PrepairProjectForCreation(It.IsAny<Project>()));

            mockAlmProvider
                .Setup(e => e.GetAssigneesNames(It.IsAny<int>()))
                .Returns(new List<Assignee>() { new Assignee() { ProjectId = 1, Name = "assigne 1" }, });

            mockAlmProvider
                .Setup(e => e.GetBugs(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Bug>() { new Bug() { ProjectId = 1, BugName = "bug 1" }, });

            mockAlmProvider
                .Setup(e => e.GetEpics(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Epic>() { new Epic() { ProjectId = 1, EpicName = "Epic 1" }, });

            mockAlmProvider
                .Setup(e => e.GetFeatures(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<Feature>() { new Feature() { ProjectId = 1, FeatureName = "Feature 1" }, });

            mockAlmProvider
                .Setup(e => e.GetIterationsList(It.IsAny<int>()))
                .Returns(new List<Iteration>() { new Iteration() { ProjectId = 1, IterationName = "Iteration 1" }, });

            mockAlmProvider
                .Setup(e => e.GetTasks(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<ALM.Task>() { new ALM.Task() { ProjectId = 1, TaskName = "Task 1" }, });

            mockAlmProvider
                .Setup(e => e.GetUserStories(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new List<UserStory>() { new UserStory() { ProjectId = 1, UserStoryName = "User Story 1" }, });




            mockAIReportsProvider
                .Setup(e => e.CheckWorkspaceExist(It.IsAny<string>()))
                .Returns(System.Threading.Tasks.Task.FromResult(false));

            mockAIReportsProvider
                .Setup(e => e.CreateWorkspace(It.IsAny<string>()))
                .Returns(System.Threading.Tasks.Task.FromResult(new Group() { Id = "1", Name = "G1" }));

            mockAIReportsProvider
                .Setup(e => e.GetAccessToken())
                .Returns("dahsdfasgdfasghdfasghfdghsafdghas");

            mockAIReportsProvider
                .Setup(e => e.GetAllWorkspaces())
                .Returns(new List<Group>() { new Group() { Id = "2", Name = "G 1" }, });

            mockAIReportsProvider
                .Setup(e => e.GetReports());


            UsingDbContext(context =>
            {
                var data = new List<Project>
                { new Project
                    {
                        Id = 5,
                        ProjectId = 5,
                        Password = "sdasdas",
                        UserName = "sdawww",
                        AuthType = (int)AuthenticationTypeEnum.AlternativePassword,
                    },
                };
                context.Project.AddRange(data);
            });
            this.projectAppService = Resolve<IProjectAppService>();
        }

        [Fact]
        public async System.Threading.Tasks.Task RegisterNewProject_ValidProject_ShouldBeAdded()
        {
            // Arrange
            var project = new ProjectDto
            {
                ProjectName = "test",
                AccountId = 14,
                UserName = "Mahmoud.Sayed@integrant.com",
                Password = "rhmm6igwfgc7tozpianifzqnonx7twimix2ahplaqojpfe2rbrpa",
                OnlineTfsCollectionUri = "https://dev.azure.com/MahmoudSayed0288",
                CollectionName = " ",
                ReleaseName = "AEM",
                IsOnPremProject = false,
                AuthType = 2,
                WorkspaceName = "Integrant Workspace",
                FromDate = DateTime.Parse("2019-06-13T16:32:18.126Z"),
                ToDate = DateTime.Parse("2019-07-13T16:32:18.126Z"),
                Id = 0
            };

            // Act
            var projectDetailDto = await projectAppService.Create(project).ConfigureAwait(false);

            // Assert
            projectDetailDto.ShouldBeOfType<ProjectDto>().ShouldNotBeNull();
            projectDetailDto.ProjectId.ShouldBe<int>(1);

        }

        [Fact]
        public async System.Threading.Tasks.Task RegisterNewProject_InvalidProject_ValidationError()
        {
            // Arrange
            var project = new ProjectDto
            {
                Id = 6,
                ProjectId = 6,
                IsOnPremProject = false,
                Password = "sdasdas",
                UserName = "sdawww",
                AuthType = (int)AuthenticationTypeEnum.AlternativePassword,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now
            };

            // Act & Assert
            Should.Throw<AbpValidationException>(async () => await projectAppService.Create(project).ConfigureAwait(false));

        }

        [Fact]
        public async System.Threading.Tasks.Task RegisterNewProject_InvalidProject_DateValidationError()
        {
            // Arrange
            var project = new ProjectDto
            {
                ProjectName = "test",
                AccountId = 14,
                UserName = "Mahmoud.Sayed@integrant.com",
                Password = "rhmm6igwfgc7tozpianifzqnonx7twimix2ahplaqojpfe2rbrpa",
                OnlineTfsCollectionUri = "https://dev.azure.com/MahmoudSayed0288",
                CollectionName = " ",
                ReleaseName = "AEM",
                IsOnPremProject = false,
                AuthType = 2,
                WorkspaceName = "Integrant Workspace",
                FromDate = DateTime.Parse("2019-08-13T16:32:18.126Z"),
                ToDate = DateTime.Parse("2019-07-13T16:32:18.126Z"),
                Id = 0
            };

            // Act & Assert
            Should.Throw<AbpValidationException>(async () => await projectAppService.Create(project).ConfigureAwait(false));
        }
    }
}
