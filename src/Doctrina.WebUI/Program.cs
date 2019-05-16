using Doctrina.Application.Interfaces;
using Doctrina.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace Doctrina.WebUI
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
                //.ReadFrom.AppSettings()
                .WriteTo.RollingFile("logs/log-{Date}.log",
                    shared: true,
                    flushToDiskInterval: new TimeSpan(0, 0, 5))
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
                //.WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Error)
                //.ReadFrom.Configuration(Configuration)
                //.Enrich.WithProperty("AppName", "Doctrina LRS")
                .CreateLogger();

            try
            {
                var host = BuildWebHost(args);

                using (var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<IDoctrinaDbContext>();

                        var concreteContext = (DoctrinaDbContext)context;
                        concreteContext.Database.Migrate();
                        DoctrinaInitializer.Initialize(concreteContext);
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
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
