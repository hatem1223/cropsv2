using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CROPS.Migrations
{
    public partial class intial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Confg");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Descriptions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateTable(
                name: "Assignee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RoleCode = table.Column<int>(nullable: true),
                    LevelCode = table.Column<int>(nullable: true),
                    IsActive = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssigneeLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Level = table.Column<string>(nullable: true),
                    LevelCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigneeLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssigneeRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Role = table.Column<string>(nullable: true),
                    RoleCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssigneeRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bug",
                columns: table => new
                {
                    BugId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BugSourceId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IterationSk = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    BugName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    AssignedTo = table.Column<string>(nullable: true),
                    CompletedWork = table.Column<decimal>(nullable: true),
                    OriginalEstimate = table.Column<decimal>(nullable: true),
                    RemainingWork = table.Column<decimal>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    UserStorySourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bug", x => x.BugId);
                });

            migrationBuilder.CreateTable(
                name: "Epic",
                columns: table => new
                {
                    EpicId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EpicSourceId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IterationSk = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    EpicName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epic", x => x.EpicId);
                });

            migrationBuilder.CreateTable(
                name: "Feature",
                columns: table => new
                {
                    FeatureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FeatureSourceId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IterationSk = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    FeatureName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feature", x => x.FeatureId);
                });

            migrationBuilder.CreateTable(
                name: "Iteration",
                columns: table => new
                {
                    IterationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IterationSourceId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IterationName = table.Column<string>(nullable: true),
                    Depth = table.Column<int>(nullable: true),
                    LastUpdatedDateTime = table.Column<DateTime>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    Scope = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Iteration", x => x.IterationId);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentSourceId = table.Column<int>(nullable: true),
                    ProjectSourceId = table.Column<int>(nullable: true),
                    ReleaseSourceId = table.Column<int>(nullable: true),
                    ProjectName = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ProjectAreaPath = table.Column<string>(nullable: true),
                    AccountId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    ParentName = table.Column<string>(nullable: true),
                    ProjectType = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    AuthType = table.Column<int>(nullable: true),
                    WorkspaceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ProjectId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectDetails",
                columns: table => new
                {
                    ProjectDetailsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    OptimisticFinalIteration = table.Column<int>(nullable: false),
                    PessimisticFinalIteration = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    Scope = table.Column<int>(nullable: true),
                    Logo = table.Column<byte[]>(nullable: true),
                    Descriptions = table.Column<string>(nullable: true),
                    PullDataFromDate = table.Column<DateTime>(nullable: false),
                    PullDataToDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDetails", x => x.ProjectDetailsId);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    TaskId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskSourceId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IterationSk = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    TaskName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    AssignedTo = table.Column<string>(nullable: true),
                    CompletedWork = table.Column<decimal>(nullable: true),
                    OriginalEstimate = table.Column<decimal>(nullable: true),
                    RemainingWork = table.Column<decimal>(nullable: true),
                    Activity = table.Column<string>(nullable: true),
                    UserStorySourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "UserStory",
                columns: table => new
                {
                    UserStoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserStorySourceId = table.Column<int>(nullable: true),
                    ProjectId = table.Column<int>(nullable: true),
                    IterationSk = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    UserStoryName = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    AssignedTo = table.Column<string>(nullable: true),
                    StoryPoints = table.Column<int>(nullable: true),
                    IsActive = table.Column<int>(nullable: true),
                    IsCr = table.Column<bool>(nullable: true),
                    FeatureSourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStory", x => x.UserStoryId);
                });

            migrationBuilder.CreateTable(
                name: "Workspace",
                columns: table => new
                {
                    WorkspaceId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workspace", x => x.WorkspaceId);
                });

            migrationBuilder.CreateTable(
                name: "ProjectData",
                schema: "Confg",
                columns: table => new
                {
                    ProjectDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectId = table.Column<int>(nullable: false),
                    OptimisticDate = table.Column<int>(nullable: false),
                    PessimisticDate = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectData", x => x.ProjectDataId);
                });

            migrationBuilder.CreateTable(
                name: "Dashboard",
                columns: table => new
                {
                    DashboardId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 1500, nullable: true),
                    ContainerWorkspaceWorkspaceId = table.Column<string>(nullable: true),
                    Id = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dashboard", x => x.DashboardId);
                    table.ForeignKey(
                        name: "FK_Dashboard_Workspace_ContainerWorkspaceWorkspaceId",
                        column: x => x.ContainerWorkspaceWorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "WorkspaceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ReportId = table.Column<string>(maxLength: 450, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    ContainerWorkspaceWorkspaceId = table.Column<string>(maxLength: 450, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(maxLength: 1500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_Workspace_ContainerWorkspaceWorkspaceId",
                        column: x => x.ContainerWorkspaceWorkspaceId,
                        principalTable: "Workspace",
                        principalColumn: "WorkspaceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dashboard_ContainerWorkspaceWorkspaceId",
                table: "Dashboard",
                column: "ContainerWorkspaceWorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ContainerWorkspaceWorkspaceId",
                table: "Report",
                column: "ContainerWorkspaceWorkspaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Assignee");

            migrationBuilder.DropTable(
                name: "AssigneeLevels");

            migrationBuilder.DropTable(
                name: "AssigneeRoles");

            migrationBuilder.DropTable(
                name: "Bug");

            migrationBuilder.DropTable(
                name: "Dashboard");

            migrationBuilder.DropTable(
                name: "Epic");

            migrationBuilder.DropTable(
                name: "Feature");

            migrationBuilder.DropTable(
                name: "Iteration");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "ProjectDetails");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "UserStory");

            migrationBuilder.DropTable(
                name: "ProjectData",
                schema: "Confg");

            migrationBuilder.DropTable(
                name: "Workspace");
        }
    }
}
