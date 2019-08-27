using Microsoft.EntityFrameworkCore.Migrations;

namespace CROPS.Migrations
{
    public partial class ReportsIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Report",
            //    table: "Report");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Workspace");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Dashboard");

            migrationBuilder.AlterColumn<string>(
                name: "ReportId",
                table: "Report",
                maxLength: 450,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Workspace",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReportId",
                table: "Report",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Report",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Dashboard",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");
        }
    }
}
