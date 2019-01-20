using Doctrina.xAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Converters
{
    public class ContextActivitiesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ContextActivities);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            JObject jobj = JObject.Load(reader);

            //var properties = jobj.Properties();
            //string[] names = new string[]
            //{
            //    "parent", "grouping", "category", "other"
            //};
            //if (properties.Any(x => !names.Contains(x.Name)))
            //    throw new JsonSerializationException("Only \"parent\", \"grouping\", \"category\", or \"other\"");

            var context = new ContextActivities();

            foreach(var obj in jobj)
            {
                var activities = new List<Activity>();
                if(obj.Value.Type == JTokenType.Array)
                {
                    activities.AddRange(obj.Value.ToObject<List<Activity>>(serializer));
                }
                else if(obj.Value.Type == JTokenType.Object)
                {
                    activities.Add(obj.Value.ToObject<Activity>(serializer));
                }
                switch (obj.Key)
                {
                    case "parent":
                        context.Parent = activities.ToArray();
                        break;
                    case "grouping":
                        context.Grouping = activities.ToArray();
                        break;
                    case "category":
                        context.Category = activities.ToArray();
                        break;
                    case "other":
                        context.Other = activities.ToArray();
                        break;
                    default:
                        throw new JsonSerializationException("Unknown context activity .");
                }
            }

            return context;
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanWrite => false;
    }
}
