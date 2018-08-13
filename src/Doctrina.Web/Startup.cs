using Doctrina.Core;
using Doctrina.Core.Data;
using Doctrina.Core.Repositories;
using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Mvc.Formatters;
using Doctrina.xAPI.Json.Converters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Schema;

namespace Doctrina.Web
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            _logger.LogInformation("Configuring DB");
            services.AddDbContext<DoctrinaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DoctrinaContext"))
                //options.UseInMemoryDatabase("Doctrina")
                );


            services.AddIdentity<DoctrinaUser, IdentityRole>()
                .AddEntityFrameworkStores<DoctrinaContext>()
                .AddDefaultTokenProviders();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.1#annotate-class-with-apicontrollerattribute
            services.AddMvc(opt =>
            {
                // Add input formatter. This should be inserted at position 0 or else the normal json input
                // formatter will take precedence.
                opt.InputFormatters.Insert(0, new StatementsInputFormatter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(opt =>
            {
                opt.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                opt.SerializerSettings.Converters.Insert(0, new UriJsonConverter());
                opt.SerializerSettings.Converters.Add(new LanguageMapJsonConverter());
            });

            services.AddCors();

            services.AddHttpContextAccessor();

            // Add application repositories.
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityProfileRepository, ActivityProfileRepository>();
            services.AddScoped<IActivityStateRepository, ActivityStateRepository>();
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IAgentProfileRepository, AgentProfileRepository>();
            //services.AddScoped<IAuthTokenRepository, AuthTokenRepository>();
            services.AddScoped<ISubStatementRepository, SubStatementRepository>();
            services.AddScoped<IVerbRepository, VerbRepository>();

            // Add application services.
            services.AddScoped<ISubStatementService, SubStatementService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IStatementService, StatementService>();
            services.AddScoped<IActivityProfileService, ActivityProfileService>();
            services.AddScoped<IActivityStateService, ActivityStateService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAgentProfileService, AgentProfileService>();
            //services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IVerbService, VerbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddFile("Logs/doctrina-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            }

            loggerFactory.AddDebug();

            //app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            License.RegisterLicense("3649-sZ/v2JAi0b1NeuXTAzlwEZDpBfPynHQ1+xaoVJNqk7jB+WSJvnHGbt8eioWr83LS6CT10w4lsQsQ5F7j7j4NUWGdKyc84xS8zrhsuHGI7Q5J55qwV9aSdJ/oGaBwBVVfZJQcFT33l0+oRTMCC2RBeipRQAFv36wejqM8OeUwj8V7IklkIjozNjQ5LCJFeHBpcnlEYXRlIjoiMjAxOS0wNC0yOVQxNTo1NjozNi44OTcyOTE3WiIsIlR5cGUiOiJKc29uU2NoZW1hSW5kaWUifQ==");
        }
    }
}
