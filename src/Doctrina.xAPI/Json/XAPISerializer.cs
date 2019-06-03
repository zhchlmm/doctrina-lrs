
namespace Doctrina.xAPI.Json
{
    public class ApiJsonSerializer : Newtonsoft.Json.JsonSerializer
    {
        public ApiVersion Version { get; }
        public ResultFormat ResultFormat { get; }

        public ApiJsonSerializer()
            : this(ApiVersion.GetLatest())
        {
        }

        public ApiJsonSerializer(ApiVersion version)
            : this(version, ResultFormat.Exact)
        {
        }

        public ApiJsonSerializer(ApiVersion version, ResultFormat format)
        {
            Version = version;
            ResultFormat = format;
            CheckAdditionalContent = true;
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;
            DateParseHandling = Newtonsoft.Json.DateParseHandling.None;
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
        }
    }
}
