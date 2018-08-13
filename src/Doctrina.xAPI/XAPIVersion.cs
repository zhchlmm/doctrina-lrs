using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonConverter(typeof(XAPIVersionConverter))]
    public sealed class XAPIVersion
    {
        private string _version;

        public static readonly XAPIVersion V103 = new XAPIVersion("1.0.3");
        public static readonly XAPIVersion V102 = new XAPIVersion("1.0.2");
        public static readonly XAPIVersion V101 = new XAPIVersion("1.0.1");
        public static readonly XAPIVersion V100 = new XAPIVersion("1.0.0");
        public static readonly XAPIVersion V095 = new XAPIVersion("0.95");
        public static readonly XAPIVersion V090 = new XAPIVersion("0.9");

        public static XAPIVersion Latest()
        {
            return V101;
        }

        private static Dictionary<string, XAPIVersion> KnownVersions;
        private static Dictionary<string, XAPIVersion> SupportedVersions;

        public static Dictionary<string, XAPIVersion> GetKnownVersions()
        {
            if (KnownVersions != null)
            {
                return KnownVersions;
            }

            KnownVersions = new Dictionary<string, XAPIVersion>
            {
                { "1.0.3", V103 },
                { "1.0.2", V102 },
                { "1.0.1", V101 },
                { "1.0.0", V100 },
                { "0.95", V095 },
                { "0.9", V090 }
            };

            return KnownVersions;
        }

        public static Dictionary<string, XAPIVersion> GetSupported()
        {
            if (SupportedVersions != null)
            {
                return SupportedVersions;
            }

            SupportedVersions = new Dictionary<string, XAPIVersion>
            {
                { "1.0.3", V103 },
                { "1.0.2", V102 },
                { "1.0.1", V101 },
                { "1.0.0", V100 }
            };

            return SupportedVersions;
        }


        public XAPIVersion(string version)
        {
            _version = version;
        }

        public static implicit operator XAPIVersion(string version)
        {
            var s = GetKnownVersions();

            /// https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#lrs-requirements-2
            if (version.StartsWith("1.0."))
            {
                return new XAPIVersion(version);
            }

            if (!s.ContainsKey(version))
            {
                throw new ArgumentException("Unrecognized version: " + version);
            }

            return s[version];
        }

        public override string ToString()
        {
            return _version;
        }

        public override bool Equals(object obj)
        {
            var version = obj as XAPIVersion;
            return version != null &&
                   _version == version._version;
        }

        public override int GetHashCode()
        {
            return -930009502 + EqualityComparer<string>.Default.GetHashCode(_version);
        }

        public static bool operator ==(XAPIVersion version1, XAPIVersion version2)
        {
            return EqualityComparer<XAPIVersion>.Default.Equals(version1, version2);
        }

        public static bool operator !=(XAPIVersion version1, XAPIVersion version2)
        {
            return !(version1 == version2);
        }
    }
}
