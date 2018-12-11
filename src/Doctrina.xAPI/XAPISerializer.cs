using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;

namespace Doctrina.xAPI
{
    public class XAPISerializer : JsonSerializer
    {
        public XAPIVersion Version { get; }
        public ResultFormats ResultFormat { get; }

        public XAPISerializer(XAPIVersion version)
        {
            Version = version;
            ResultFormat = ResultFormats.Exact;
            CheckAdditionalContent = true;
            Converters.Insert(0, new UriJsonConverter());
        }

        public XAPISerializer(XAPIVersion version, ResultFormats format)
        {
            Version = version;
            ResultFormat = format;
            CheckAdditionalContent = true;
            Converters.Insert(0, new UriJsonConverter());
        }
    }
}
