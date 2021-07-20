using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace RobotWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuiler =>
                {
                    webBuiler.UseKestrel();
                    webBuiler.UseUrls("http://0.0.0.0:5000");
                    webBuiler.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuiler.UseIISIntegration();
                    webBuiler.UseStartup<Startup>();
                });
    }
}
