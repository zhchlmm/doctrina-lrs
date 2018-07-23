using Doctrina.Core;
using Doctrina.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using xAPI.LRS.Persistence;

namespace xAPI.LRS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<DoctrinaDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DoctrinaDbContext>()
                .AddDefaultTokenProviders();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();

            // Add application services.
            services.AddScoped<IStatementRepository, StatementRepository>();
            services.AddScoped<IActivityProfileRepository, ActivityProfileRepository>();
            services.AddScoped<IActivityStateRepository, ActivityStateRepository>();
            services.AddScoped<IAgentRepository, AgentRepository>();
            services.AddScoped<IAgentsProfileRepository, AgentsProfileRepository>();
            services.AddScoped<IAuthTokenRepository, AuthTokenRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<ISubStatementRespository, SubStatementRespository>();
            services.AddScoped<IVerbRepository, VerbRepository>();


            services.AddScoped<IStatementService, StatementService>();
            services.AddScoped<IActivityProfileService, ActivityProfileService>();
            services.AddScoped<IActivityStateService, ActivityStateService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IAgentsProfileService, AgentsProfileService>();
            services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ISubStatementService, SubStatementService>();
            services.AddScoped<IVerbService, VerbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }

    internal class ApplicationUser
    {
    }
}
