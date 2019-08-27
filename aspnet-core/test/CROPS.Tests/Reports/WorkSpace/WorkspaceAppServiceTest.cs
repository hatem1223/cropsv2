using CROPS.Reports;
using CROPS.Reports.Contracts;
using CROPS.Reports.DTOs;
using CROPS.WorkSpace.DTOs;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CROPS.Tests.Reports.WorkSpace
{
    public class WorkspaceAppServiceTest : CROPSTestBase
    {
        private readonly IWorkspaceAppService workspaceAppService;

        public WorkspaceAppServiceTest()
        {
            workspaceAppService = Resolve<IWorkspaceAppService>();

            // Arrange
            UsingDbContext(context =>
            {
                // var data = A.ListOf<ProjectDetail>(5);
                List<Workspace> data = new List<Workspace>
                {
                    new Workspace { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Workspace 1", Dashboards = new List<CROPS.Reports.Dashboard>(),Reports = new List<CROPS.Reports.Report>() },
                    new Workspace { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Workspace 2", Dashboards = new List<CROPS.Reports.Dashboard>(),Reports = new List<CROPS.Reports.Report>() },
                    new Workspace { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Workspace 3", Dashboards = new List<CROPS.Reports.Dashboard>(),Reports = new List<CROPS.Reports.Report>() },
                    new Workspace { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Workspace 4", Dashboards = new List<CROPS.Reports.Dashboard>(),Reports = new List<CROPS.Reports.Report>() },
                    new Workspace { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Workspace 0", Dashboards = new List<CROPS.Reports.Dashboard>(),Reports = new List<CROPS.Reports.Report>() },
                };
                context.Workspace.AddRange(data);
            });
        }

        [Fact]
        public void GetWorkspaces_Test()
        {
            // Act
            List<WorkspaceDto> workspaces = workspaceAppService.GetAll();

            // Assert
            //workspaces.ShouldNotBe(null);
            //workspaces.Count.ShouldBeGreaterThan(0);
            workspaces.Count.ShouldBe(5);
            workspaces.ShouldBeOfType<List<WorkspaceDto>>().ShouldNotBeNull();
        }
    }
}
