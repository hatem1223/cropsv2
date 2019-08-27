using CROPS.Configuration;
using CROPS.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CROPS.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class CROPSDbContextFactory : IDesignTimeDbContextFactory<CROPSDbContext>
    {
        public CROPSDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CROPSDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            CROPSDbContextConfigurer.Configure(builder, configuration.GetConnectionString(CROPSConsts.ConnectionStringName));

            return new CROPSDbContext(builder.Options);
        }
    }
}
