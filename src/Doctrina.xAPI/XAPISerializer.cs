using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI
{
    public class XAPISerializer : JsonSerializer
    {
        public XAPIVersion Version { get; }

        public XAPISerializer(XAPIVersion version)
        {
            Version = version;
            CheckAdditionalContent = true;
            Converters.Insert(0, new UriJsonConverter());
        }
    }
}
