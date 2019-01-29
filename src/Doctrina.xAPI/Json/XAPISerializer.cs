using Doctrina.xAPI.Json.Converters;
using Doctrina.xAPI.Json.Serialization;
using Newtonsoft.Json.Serialization;

namespace Doctrina.xAPI.Json
{
    public class ApiJsonSerializer : Newtonsoft.Json.JsonSerializer
    {
        public ApiVersion Version { get; }
        public ResultFormats ResultFormat { get; }

        public ApiJsonSerializer(ApiVersion version)
            : this(version, ResultFormats.Exact)
        {
        }

        public ApiJsonSerializer(ApiVersion version, ResultFormats format)
        {
            Version = version;
            ResultFormat = format;
            CheckAdditionalContent = true;
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Error;
            Converters.Insert(0, new StrictNumberConverter());
            Converters.Insert(0, new StrictStringConverter());
            Converters.Insert(1, new UriJsonConverter());
            Converters.Insert(2, new DateTimeJsonConverter());
        }
    }
}
