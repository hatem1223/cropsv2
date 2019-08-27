using Microsoft.EntityFrameworkCore.Migrations;

namespace CROPS.Migrations
{
    public partial class AccountId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContainerWorkspaceWorkspaceId",
                table: "Dashboard",
                maxLength: 450,
                nullable: true);
        }
    }
}
