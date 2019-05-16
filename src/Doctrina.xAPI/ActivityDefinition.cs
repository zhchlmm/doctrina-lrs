using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public class ActivityDefinition : JsonModel
    {
        public ActivityDefinition()
        {
        }
        public ActivityDefinition(string jsonString) :
            this(JObject.Parse(jsonString))
        {
        }

        public ActivityDefinition(JObject jobj) :
           this(jobj, ApiVersion.GetLatest())
        {
        }

        public ActivityDefinition(JObject obj, ApiVersion version)
        {
            if (obj["type"] != null)
            {
                Type = obj["type"].Value<Iri>();
            }

            if (obj["moreInfo"] != null)
            {
                MoreInfo = obj["moreInfo"].Value<Uri>();
            }

            if (obj["description"] != null)
            {
                Description = obj.Value<JObject>("description");
            }
        }

        

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

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();

            if(Type != null)
            {
                obj["type"] = Type.ToString();
            }

            if(Name != null)
            {
                obj["name"] = Name.ToJToken(version, format);
            }

            if(Description != null)
            {
                obj["description"] = Description.ToJToken(version, format);
            }

            if(MoreInfo != null)
            {
                obj["moreInfo"] = MoreInfo.ToString();
            }

            obj["extentions"] = Extentions?.ToJToken(version, format);

            return obj;
        }

        internal static ActivityDefinition Parse(JObject jObject, ApiVersion version)
        {
            throw new NotImplementedException();
        }
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

        public static implicit operator ActivityDefinition(JObject jobj)
        {
            return new ActivityDefinition(jobj);
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