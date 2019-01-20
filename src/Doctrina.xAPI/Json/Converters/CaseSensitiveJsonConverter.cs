using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Doctrina.xAPI.Json.Converters
{
    public class CaseSensitiveJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsPrimitive;
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var properties = objectType
                .GetProperties();

            var token = JToken.Load(reader);

            foreach(var property in properties)
            {
                string name = property.Name;

                var jsonPropertyAttributes = (JsonPropertyAttribute[])property.GetCustomAttributes(typeof(JsonPropertyAttribute), true);
                if(jsonPropertyAttributes != null && jsonPropertyAttributes.Count() == 1)
                {
                    var jsonPropertyAttribute = jsonPropertyAttributes.SingleOrDefault();
                    name = jsonPropertyAttribute.PropertyName;
                }
            }
                
            return token.ToObject(objectType);
        }

        public override bool CanWrite => false; 

        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            JObject o = (JObject)JToken.FromObject(value);
            o.WriteTo(writer);
        }

        private static void WalkNode(JToken node,
                                Action<JObject> objectAction = null,
                                Action<JProperty> propertyAction = null)
        {
            if (node.Type == JTokenType.Object)
            {
                if (objectAction != null) objectAction((JObject)node);
                foreach (JProperty child in node.Children<JProperty>())
                {
                    if (propertyAction != null) propertyAction(child);
                    WalkNode(child.Value, objectAction, propertyAction);
                }
            }
            else if (node.Type == JTokenType.Array)
                foreach (JToken child in node.Children())
                    WalkNode(child, objectAction, propertyAction);
        }
    }
}
