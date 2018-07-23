using ExperienceAPI.Core.Models;
using ExperienceAPI.Core.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using System.Web.Http.ValueProviders.Providers;

namespace UmbracoLRS.Core.ModelBinders
{
    public class JsonFromUriAttribute : ModelBinderAttribute
    {
        public JsonFromUriAttribute()
            : base()
        {
        }

        public override IEnumerable<ValueProviderFactory> GetValueProviderFactories(HttpConfiguration configuration)
        {
            var valueProviders = base.GetValueProviderFactories(configuration);
            return valueProviders;
        }

        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            var binding = base.GetBinding(parameter);

            return binding;
        }
    }
}
