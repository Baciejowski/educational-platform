using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            CreateHostBuilder(config, args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(IConfigurationRoot config, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5000);
                        options.Listen(IPAddress.Any, 5001, listenOptions =>
                        {
                            listenOptions.UseHttps(config.GetSection("Cert")["File"], config.GetSection("Cert")["Password"]);
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
