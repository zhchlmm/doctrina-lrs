using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Doctrina.Web
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
            // Add framework services.
            //_logger.LogInformation("Configuring DB");
            //services.AddDbContext<DoctrinaContext>(options =>
            //        //options.UseSqlServer(Configuration.GetConnectionString("DoctrinaContext"))
            //        options.UseInMemoryDatabase("Doctrina")
            //    );

            //services.AddIdentity<DoctrinaUser, IdentityRole>()
            //    .AddEntityFrameworkStores<DoctrinaContext>()
            //    .AddDefaultTokenProviders();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //loggerFactory.AddFile("Logs/doctrina-{Date}.txt");

            if (env.IsDevelopment())
            {
                _logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                //app.UseHsts();
                app.UseExceptionHandler("/error");
                //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            }

            //loggerFactory.AddDebug();

            //app.UseHttpsRedirection();


            // Return static files and end the pipeline.
            app.UseStaticFiles();

            // Use Cookie Policy Middleware to conform to EU General Data 
            // Protection Regulation (GDPR) regulations.
            app.UseCookiePolicy();

            // Authenticate before the user accesses secure resources.
            app.UseAuthentication();

            // If the app uses session state, call Session Middleware after Cookie 
            // Policy Middleware and before MVC Middleware.
            //app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
