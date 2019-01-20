using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Serialization
{
    public class OptInNamesContractResolver : DefaultContractResolver
    {
        public OptInNamesContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy
            {
                OverrideSpecifiedNames = false
            };
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            return properties;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            property.ShouldDeserialize = t =>
            {
                var jsonPropertyAttributes = t.GetType().GetCustomAttributes<Newtonsoft.Json.JsonPropertyAttribute>();
                if(jsonPropertyAttributes != null && jsonPropertyAttributes.Any())
                {
                    var jsonPropertyAttribute = jsonPropertyAttributes.SingleOrDefault();
                    if(!property.PropertyName.Equals(jsonPropertyAttribute.PropertyName, StringComparison.Ordinal))
                    {
                        return false;
                    }
                }
                return true;
            };

            return property;
        }

        private JsonProperty MatchProperty(JsonPropertyCollection properties, string name, Type type)
        {
            // it is possible to generate a member with a null name using Reflection.Emit
            // protect against an ArgumentNullException from GetClosestMatchProperty by testing for null here
            if (name == null)
            {
                return null;
            }

            JsonProperty property = properties.GetClosestMatchProperty(name);

            if(!property.PropertyName.Equals(name, StringComparison.Ordinal))
            {
                return null;
            }

            // must match type as well as name
            if (property == null || property.PropertyType != type)
            {
                return null;
            }

            return property;
        }
    }

}
