using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json
{
    public abstract class ApiJsonConverter : JsonConverter
    {
        public override abstract object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer);
        public override abstract void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer);
    }
}
