using CROPS.Reports;
using CROPS.Reports.Contracts;
using CROPS.Reports.DTOs;
using CROPS.WorkSpace.DTOs;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace CROPS.Tests.Reports.Report
{
    public class ReportAppServiceTest : CROPSTestBase
    {
        private readonly IReportAppService reportAppService;
        private Mock<IReportsProvider> reportProvider = new Mock<IReportsProvider>();

        public ReportAppServiceTest()
        {
            RegisterInstance(reportProvider.Object);
            reportAppService = Resolve<IReportAppService>();

            reportProvider
                .Setup(e => e.GetReportUrlAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(System.Threading.Tasks.Task.FromResult(string.Empty));

            // Arrange
            UsingDbContext(context =>
            {
                // var data = A.ListOf<ProjectDetail>(5);
                List<CROPS.Reports.Report> data = new List<CROPS.Reports.Report>
                {
                    new CROPS.Reports.Report { Id = "0a0c9cbe-b819-4e7c-9172-8b2b0f176f99", ReportId = "0a0c9cbe-b819-4e7c-9172-8b2b0f176f99", IsActive = true, Name = "Report 1", WorkspaceId = "010fd0a9-c71f-4799-be61-308cf64ef731" },
                    new CROPS.Reports.Report { Id = Guid.NewGuid().ToString(), ReportId = Guid.NewGuid().ToString(), IsActive = true, Name = "Report 0", WorkspaceId = Guid.NewGuid().ToString() },
                    new CROPS.Reports.Report { Id = Guid.NewGuid().ToString(), ReportId = Guid.NewGuid().ToString(), IsActive = true, Name = "Report 2", WorkspaceId = Guid.NewGuid().ToString() },
                    new CROPS.Reports.Report { Id = Guid.NewGuid().ToString(), ReportId = Guid.NewGuid().ToString(), IsActive = true, Name = "Report 3", WorkspaceId = Guid.NewGuid().ToString() },
                    new CROPS.Reports.Report { Id = Guid.NewGuid().ToString(), ReportId = Guid.NewGuid().ToString(), IsActive = true, Name = "Report 4", WorkspaceId = Guid.NewGuid().ToString() }
                };
                List<CROPS.Projects.Project> project = new List<CROPS.Projects.Project>
                {
                    new CROPS.Projects.Project { Id = 1, WorkspaceId = "010fd0a9-c71f-4799-be61-308cf64ef731", IsActive = true },
                    new CROPS.Projects.Project { Id = 2, WorkspaceId = Guid.NewGuid().ToString(), IsActive = true },
                    new CROPS.Projects.Project { Id = 3, WorkspaceId = Guid.NewGuid().ToString(), IsActive = true },
                    new CROPS.Projects.Project { Id = 4, WorkspaceId = Guid.NewGuid().ToString(), IsActive = true },
                    new CROPS.Projects.Project { Id = 5, WorkspaceId = Guid.NewGuid().ToString(), IsActive = true }
                };
                context.Report.AddRange(data);
                context.Project.AddRange(project);
            });
        }

        [Theory]
        [InlineData("0a0c9cbe-b819-4e7c-9172-8b2b0f176f99")]
        public void GetReport_Test(string id)
        {
            // Act
            CROPS.Reports.DTOs.ReportDetailsDto report = reportAppService.Get(id);

            // Assert
            report.ShouldNotBe(null);
            report.ShouldBeOfType<ReportDetailsDto>().ShouldNotBeNull();
        }
    }
}
