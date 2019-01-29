using Newtonsoft.Json.Serialization;
using System;

namespace Doctrina.xAPI.Json.Serialization
{
    public class CaseSensitiveJsonPropertyCollection : JsonPropertyCollection
    {
        public CaseSensitiveJsonPropertyCollection(Type type) 
            : base(type)
        {
        }

        /// <summary>
        /// Gets the closest matching <see cref="JsonProperty"/> object.
        /// First attempts to get an exact case match of <paramref name="propertyName"/> and then
        /// a case insensitive match.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>A matching property if found.</returns>
        public new JsonProperty GetClosestMatchProperty(string propertyName)
        {
            return GetProperty(propertyName, StringComparison.Ordinal);
        }
    }
}
