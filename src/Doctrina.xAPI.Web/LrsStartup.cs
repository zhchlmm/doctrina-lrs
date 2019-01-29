using Doctrina.Persistence;
using Doctrina.Persistence.Entities;
using Doctrina.Persistence.Repositories;
using Doctrina.Persistence.Services;
using Doctrina.xAPI.LRS.Mvc.ModelBinding.Providers;
using Doctrina.xAPI.LRS.Routing;
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

namespace Doctrina.xAPI.LRS
{
    public class LrsStartup
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger _logger;

        public LrsStartup(IConfiguration configuration, ILogger<LrsStartup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring DB");
            services.AddDbContext<DoctrinaDbContext>(options =>
                    //options.UseSqlServer(Configuration.GetConnectionString("DoctrinaContext"))
                    options.UseInMemoryDatabase("Doctrina")
                );

            _logger.LogInformation("Configuring Identity");
            services.AddIdentity<DoctrinaUser, IdentityRole>()
                .AddEntityFrameworkStores<DoctrinaDbContext>()
                .AddDefaultTokenProviders();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(opt =>
            {
                // Add input formatter. This should be inserted at position 0 or else the normal json input
                // formatter will take precedence.
                //opt.InputFormatters.Insert(0, new StatementsInputFormatter());
                //opt.InputFormatters.Insert(1, new StringInputFormatter());
                //opt.InputFormatters.Insert(0, new BytesInputFormatter());
                opt.ModelBinderProviders.Insert(0, new AgentModelBinderProvider());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(opt =>
            {
                // TODO: Are these Converters required?
                //opt.SerializerSettings.Converters.Insert(0, new UriJsonConverter());
                //opt.SerializerSettings.Converters.Add(new LanguageMapJsonConverter());
            });

            //services.AddCors();

            // Add application repositories.
            _logger.LogInformation("Configuring LRS Repositories");
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IActivityProfileRepository, ActivityProfileRepository>();
            services.AddScoped<IActivitiesStateRepository, ActivityStateRepository>();
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IAgentProfileRepository, AgentProfileRepository>();
            //services.AddScoped<IAuthTokenRepository, AuthTokenRepository>();
            services.AddScoped<ISubStatementRepository, SubStatementRepository>();
            services.AddScoped<IVerbRepository, VerbRepository>();

            // Add application services.
            _logger.LogInformation("Configuring LRS Services");
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<ISubStatementService, SubStatementService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IStatementService, StatementService>();
            services.AddScoped<IActivityProfileService, ActivityProfileService>();
            services.AddScoped<IActivitiesStateService, ActivitiesStateService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAgentProfileService, AgentProfileService>();
            //services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IVerbService, VerbService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //app.UseStatusCodePages();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseAlternateRequestSyntax();
            app.UseConsistentThrough();
            app.UseUnrecognizedParameters();

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "xapi",
                    template: "xapi/{controller}"
                );
            });

            // TODO: This license should be removed at some point.
            License.RegisterLicense("3649-sZ/v2JAi0b1NeuXTAzlwEZDpBfPynHQ1+xaoVJNqk7jB+WSJvnHGbt8eioWr83LS6CT10w4lsQsQ5F7j7j4NUWGdKyc84xS8zrhsuHGI7Q5J55qwV9aSdJ/oGaBwBVVfZJQcFT33l0+oRTMCC2RBeipRQAFv36wejqM8OeUwj8V7IklkIjozNjQ5LCJFeHBpcnlEYXRlIjoiMjAxOS0wNC0yOVQxNTo1NjozNi44OTcyOTE3WiIsIlR5cGUiOiJKc29uU2NoZW1hSW5kaWUifQ==");
        }
    }
}
