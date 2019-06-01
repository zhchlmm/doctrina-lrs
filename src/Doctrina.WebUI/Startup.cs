using AutoMapper;
using Doctrina.Application.Infrastructure;
using Doctrina.Application.Infrastructure.AutoMapper;
using Doctrina.Application.Interfaces;
using Doctrina.Application.Statements;
using Doctrina.Application.Statements.Commands;
using Doctrina.Persistence;
using Doctrina.WebUI.Filters;
using Doctrina.xAPI.LRS.Builder;
using FluentValidation.AspNetCore;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NSwag.AspNetCore;
using System.Reflection;

namespace Doctrina.WebUI
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add AutoMapper
            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });

            //services.AddCustomMapper();

            // Add framework services.
            //services.AddTransient<INotificationService, NotificationService>();
            //services.AddTransient<IDateTime, MachineDateTime>();

            // Add httpcontext
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add MediatR
            services.AddMediatR(typeof(CreateStatementCommand).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            // Add DbContext using SQL Server Provider
#if DEBUG
            services.AddDbContext<IDoctrinaDbContext, DoctrinaDbContext>(options =>
                options.UseInMemoryDatabase("Doctrina"));
#else
            services.AddDbContext<IDoctrinaDbContext, DoctrinaDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DoctrinaDatabase")));
#endif

            //services.AddIdentity<DoctrinaUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DoctrinaContext>()
            //    .AddDefaultTokenProviders();

            services.AddMvc(options => options.Filters.Add(typeof(CustomExceptionFilterAttribute)))
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateStatementsCommandValidator>());

            // Customise default API behavour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            // In production, the Vue files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/public";
            });

            services.AddLearningRecordStore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //loggerFactory.AddFile("Logs/doctrina-{Date}.txt");

            if (env.IsDevelopment())
            {
                _logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseLearningRecordStore();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });


            // Use Cookie Policy Middleware to conform to EU General Data 
            // Protection Regulation (GDPR) regulations.
            app.UseCookiePolicy();

            // Authenticate before the user accesses secure resources.
            //app.UseAuthentication();

            // If the app uses session state, call Session Middleware after Cookie 
            // Policy Middleware and before MVC Middleware.
            //app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

        }
    }
}
