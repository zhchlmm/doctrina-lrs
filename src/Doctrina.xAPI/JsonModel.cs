using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI
{
    public abstract class JsonModel : JsonModel<JObject> { }

    public abstract class JsonModel<TToken>
        where TToken : JToken
    {
        public abstract TToken ToJToken(ApiVersion version, ResultFormat format);

        public virtual string ToJson(ApiVersion version, ResultFormat format) {
            return ToJToken(version, format).ToString();
        }

        public virtual string ToJson(ResultFormat format = ResultFormat.Exact)
        {
            return ToJson(ApiVersion.GetLatest(), format);
        }

        //public static TModel Parse(string json);

        //public static bool TryParse(string json, out TModel result);

        // Causes loop reference
        //public override string ToString()
        //{
        //    return ToJson();
        //}

        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
