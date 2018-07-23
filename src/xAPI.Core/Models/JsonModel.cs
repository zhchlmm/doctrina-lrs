using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models
{
    [JsonObject]
    public abstract class JsonModel
    {
        public string ToJson(bool pretty = false)
        {
            return JsonConvert.SerializeObject(this, pretty ? Formatting.Indented : Formatting.None);
        }

        public string ToJson(XAPIVersion version)
        {
            return JsonConvert.SerializeObject(this);
        }

        public JObject ToJObject()
        {
            return JObject.Parse(this.ToJson());
        }

        public override string ToString()
        {
            return ToJson();
        }

        public T Parse<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        //public bool IsValid(out IList<ValidationError> messages)
        //{
        //    return ToJObject().IsValid(Schema, out messages);
        //}

        //public JSchema Schema
        //{
        //    get
        //    {
        //        JSchemaGenerator generator = new JSchemaGenerator();
        //        generator.GenerationProviders.Add(new StringEnumGenerationProvider());
        //        var jSchema = generator.Generate(this.GetType());
        //        return jSchema;
        //    }
        //}
    }
}
