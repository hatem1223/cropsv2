using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CROPS.Migrations
{
    public partial class change_workspace_Id_foreign_key : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboard_Workspace_ContainerWorkspaceWorkspaceId",
                table: "Dashboard");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Workspace_ContainerWorkspaceWorkspaceId",
                table: "Report");

            migrationBuilder.DropTable(
                name: "ProjectData",
                schema: "Confg");

            migrationBuilder.RenameColumn(
                name: "ContainerWorkspaceWorkspaceId",
                table: "Report",
                newName: "WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ContainerWorkspaceWorkspaceId",
                table: "Report",
                newName: "IX_Report_WorkspaceId");

            migrationBuilder.RenameColumn(
                name: "ContainerWorkspaceWorkspaceId",
                table: "Dashboard",
                newName: "WorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Dashboard_ContainerWorkspaceWorkspaceId",
                table: "Dashboard",
                newName: "IX_Dashboard_WorkspaceId");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "ProjectDetails",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboard_Workspace_WorkspaceId",
                table: "Dashboard",
                column: "WorkspaceId",
                principalTable: "Workspace",
                principalColumn: "WorkspaceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Workspace_WorkspaceId",
                table: "Report",
                column: "WorkspaceId",
                principalTable: "Workspace",
                principalColumn: "WorkspaceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboard_Workspace_WorkspaceId",
                table: "Dashboard");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Workspace_WorkspaceId",
                table: "Report");

            migrationBuilder.EnsureSchema(
                name: "Confg");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "Report",
                newName: "ContainerWorkspaceWorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_WorkspaceId",
                table: "Report",
                newName: "IX_Report_ContainerWorkspaceWorkspaceId");

            migrationBuilder.RenameColumn(
                name: "WorkspaceId",
                table: "Dashboard",
                newName: "ContainerWorkspaceWorkspaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Dashboard_WorkspaceId",
                table: "Dashboard",
                newName: "IX_Dashboard_ContainerWorkspaceWorkspaceId");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Logo",
                table: "ProjectDetails",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProjectData",
                schema: "Confg",
                columns: table => new
                {
                    ProjectDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    OptimisticDate = table.Column<int>(nullable: false),
                    PessimisticDate = table.Column<int>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectData", x => x.ProjectDataId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboard_Workspace_ContainerWorkspaceWorkspaceId",
                table: "Dashboard",
                column: "ContainerWorkspaceWorkspaceId",
                principalTable: "Workspace",
                principalColumn: "WorkspaceId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Workspace_ContainerWorkspaceWorkspaceId",
                table: "Report",
                column: "ContainerWorkspaceWorkspaceId",
                principalTable: "Workspace",
                principalColumn: "WorkspaceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
