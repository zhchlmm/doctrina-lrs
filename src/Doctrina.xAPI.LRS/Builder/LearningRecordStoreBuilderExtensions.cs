using Doctrina.xAPI.Json.Converters;
using Doctrina.xAPI.LRS.Mvc.ModelBinding.Providers;
using Doctrina.xAPI.LRS.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Doctrina.xAPI.LRS.Builder
{
    public static class LearningRecordStoreBuilderExtensions
    {
        public static IServiceCollection AddLearningRecordStore(this IServiceCollection services)
        {
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
                opt.SerializerSettings.Converters.Add(new DateTimeJsonConverter());
                opt.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.None;
                opt.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.None;
                opt.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                //opt.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;
            });

            //services.AddCors();

            return services;
        }

        public static IApplicationBuilder UseLearningRecordStore(this IApplicationBuilder builder)
        {
            //app.UseStatusCodePages();
            builder.UseMiddleware<RequestResponseLoggingMiddleware>();
            builder.UseMiddleware<AlternateRequestMiddleware>();
            builder.UseMiddleware<ConsistentThroughMiddleware>();
            builder.UseMiddleware<UnrecognizedParametersMiddleware>();

            builder.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "xapi",
                    template: "xapi/{controller}"
                );
            });

            

            return builder;
        }
    }
}
