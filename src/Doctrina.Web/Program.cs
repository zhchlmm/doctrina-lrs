using Doctrina.Core;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Core;

namespace Doctrina.Web
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                //.UseKestrel(options =>
                //{
                //    options.Listen(IPAddress.Loopback, 5000);
                //})
                //.UseIISIntegration()
                .Build();

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.RollingFile("logs/log-{Date}.log", shared: true)
                //.ReadFrom.Configuration(Configuration)
                .Enrich.WithProperty("AppName", "Doctrina LRS")
                .CreateLogger();

            try
            {
                var host = BuildWebHost(args);

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<DoctrinaContext>();
                    try
                    {
                        SeedData.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred seeding the DB.");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        
    }
}
