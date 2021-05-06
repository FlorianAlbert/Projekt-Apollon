using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Apollon.Mud.Server.Inbound
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(new []
                    {
                        $"{config["Host:HTTP:URL"]}:{config["Host:HTTP:Port"]}",
                        $"{config["Host:HTTPS:URL"]}:{config["Host:HTTPS:Port"]}"

                    }).UseStartup<Startup>();
                });
        }
            
    }
}
