using CROPS.Reports;
using CROPS.Reports.Contracts;
using CROPS.Reports.DTOs;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace CROPS.Tests.Reports.Dashboard
{
    public class DashboardAppServiceTest : CROPSTestBase
    {
        private readonly IDashboardAppService dashboardAppService;
        private Mock<IReportsProvider> dashboardProvider = new Mock<IReportsProvider>();

        public DashboardAppServiceTest()
        {
            RegisterInstance(dashboardProvider.Object);
            dashboardAppService = Resolve<IDashboardAppService>();

            dashboardProvider
                .Setup(e => e.GetDashboardAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(System.Threading.Tasks.Task.FromResult(new Microsoft.PowerBI.Api.V2.Models.Dashboard() { DisplayName = string.Empty, EmbedUrl = string.Empty }));

            // Arrange
            UsingDbContext(context =>
            {
                // var data = A.ListOf<ProjectDetail>(5);
                List<CROPS.Reports.Dashboard> data = new List<CROPS.Reports.Dashboard>
                {
                    new CROPS.Reports.Dashboard { Id = "0a0c9cbe-b819-4e7c-9172-8b2b0f176f99", DashboardId = "0a0c9cbe-b819-4e7c-9172-8b2b0f176f99", WorkspaceId = "010fd0a9-c71f-4799-be61-308cf64ef731", IsActive = true, Name = "Dashboard 1" },
                    new CROPS.Reports.Dashboard { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Dashboard 0" },
                    new CROPS.Reports.Dashboard { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Dashboard 2" },
                    new CROPS.Reports.Dashboard { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Dashboard 3" },
                    new CROPS.Reports.Dashboard { Id = Guid.NewGuid().ToString(), IsActive = true, Name = "Dashboard 4" }
                };
                context.Dashboard.AddRange(data);
            });
        }

        [Theory]
        [InlineData("0a0c9cbe-b819-4e7c-9172-8b2b0f176f99")]
        public void GetDashboard_Test(string id, string userId = null)
        {
            // Act
            DashboardDetailsDto dashboard = dashboardAppService.GetDashboard(id, userId);

            // Assert
            dashboard.ShouldNotBe(null);
            dashboard.ShouldBeOfType<DashboardDetailsDto>().ShouldNotBeNull();
        }
    }
}
