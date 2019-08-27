using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace CROPS.Web.Host.Startup
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();
        }
    }
}
