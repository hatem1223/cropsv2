using Microsoft.EntityFrameworkCore.Migrations;

namespace CROPS.Migrations
{
    public partial class Create_SP_GetProjectDataByProjectAreaPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                CREATE PROCEDURE SP_GetProjectDataByProjectAreaPath @ProjectAreaPath nvarchar(600)
                                    AS
                                    BEGIN
	                                    SET NOCOUNT ON;

                                        SELECT ParentNodeSK, ProjectNodeSK, AreaSK ReleaseSourceId
                                        FROM [10.1.22.94].[Tfs_Warehouse].[dbo].[DimTeamProject]
	                                    join [10.1.22.94].[Tfs_Warehouse].[dbo].DimArea on ProjectGUID = CONVERT(NVARCHAR(MAX), ProjectNodeGUID)
                                        WHERE ProjectPath = @ProjectAreaPath;
                                    END
                                GO
                                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS SP_GetProjectDataByProjectAreaPath;");
        }
    }
}
