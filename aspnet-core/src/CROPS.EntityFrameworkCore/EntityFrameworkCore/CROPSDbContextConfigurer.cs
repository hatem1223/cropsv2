using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace CROPS.EntityFrameworkCore
{
    public static class CROPSDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<CROPSDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<CROPSDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
