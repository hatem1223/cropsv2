using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CROPS.Migrations
{
    public partial class project_detail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "ProjectData",
            //    schema: "Confg");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "ProjectDetails",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDetails_ProjectId",
                table: "ProjectDetails",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDetails_Project_ProjectId",
                table: "ProjectDetails",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDetails_Project_ProjectId",
                table: "ProjectDetails");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDetails_ProjectId",
                table: "ProjectDetails");

            migrationBuilder.EnsureSchema(
                name: "Confg");

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
        }
    }
}
