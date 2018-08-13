using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI.Models
{
    [JsonObject()]
    public class ActivityDefinition : JsonModel
    {
        [JsonProperty("name", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public LanguageMap Name { get; set; }

        [JsonProperty("description", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public LanguageMap Description { get; set; }

        [JsonProperty("type", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public virtual Iri Type { get; set; }

        /// <summary>
        /// Resolves to a document with human-readable information about the Activity, which could include a way to launch the activity.
        /// </summary>
        [JsonProperty("moreInfo", 
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Uri MoreInfo { get; set; }

        [JsonProperty("extensions", 
            NullValueHandling = NullValueHandling.Include,
            Required = Required.Default)]
        public Extensions Extentions { get; set; }

        public override bool Equals(object obj)
        {
            var definition = obj as ActivityDefinition;
            return definition != null &&
                   base.Equals(obj) &&
                   //EqualityComparer<LanguageMap>.Default.Equals(Name, definition.Name) &&
                   //EqualityComparer<LanguageMap>.Default.Equals(Description, definition.Description) &&
                   EqualityComparer<Iri>.Default.Equals(Type, definition.Type) &&
                   EqualityComparer<Uri>.Default.Equals(MoreInfo, definition.MoreInfo);
        }

        public override int GetHashCode()
        {
            var hashCode = -1346148704;
            //hashCode = hashCode * -1521134295 + EqualityComparer<LanguageMap>.Default.GetHashCode(Name);
            //hashCode = hashCode * -1521134295 + EqualityComparer<LanguageMap>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<Iri>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(MoreInfo);
            return hashCode;
        }

        public static bool operator ==(ActivityDefinition definition1, ActivityDefinition definition2)
        {
            return EqualityComparer<ActivityDefinition>.Default.Equals(definition1, definition2);
        }

        public static bool operator !=(ActivityDefinition definition1, ActivityDefinition definition2)
        {
            return !(definition1 == definition2);
        }
    }
}
