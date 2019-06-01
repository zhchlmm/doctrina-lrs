using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public class ActivityDefinition : JsonModel
    {
        public ActivityDefinition() {}
        public ActivityDefinition(JsonString jsonString) : this(jsonString.ToJToken()) {}
        public ActivityDefinition(JToken jobj) : this(jobj, ApiVersion.GetLatest()) {}
        public ActivityDefinition(JToken obj, ApiVersion version)
        {
            if (!AllowObject(obj))
            {
                return;
            }

            if (DisallowNull(obj["type"]) && AllowString(obj["type"]))
            {
                Type = new Iri(obj["type"].Value<string>());
            }

            if (DisallowNull(obj["moreInfo"]) && AllowString(obj["moreInfo"]))
            {
                MoreInfo = new Uri(obj["moreInfo"].Value<string>());
            }

            if (DisallowNull(obj["name"]))
            {
                Description = new LanguageMap(obj.Value<JToken>("name"), version);
            }

            if (DisallowNull(obj["description"]))
            {
                Description = new LanguageMap(obj.Value<JToken>("description"), version);
            }

            if (obj["extensions"] != null)
            {
                Extensions = new Extensions(obj.Value<JToken>("extensions"), version);
            }
        }

        public LanguageMap Name { get; set; }

        public LanguageMap Description { get; set; }

        public virtual Iri Type { get; set; }

        /// <summary>
        /// Resolves to a document with human-readable information about the Activity, which could include a way to launch the activity.
        /// </summary>
        public Uri MoreInfo { get; set; }

        public Extensions Extensions { get; set; }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject();

            if (Type != null)
            {
                obj["type"] = Type.ToString();
            }

            if (Name != null)
            {
                obj["name"] = Name.ToJToken(version, format);
            }

            if (Description != null)
            {
                obj["description"] = Description.ToJToken(version, format);
            }

            if (MoreInfo != null)
            {
                obj["moreInfo"] = MoreInfo.ToString();
            }

            if(Extensions != null)
            {
                obj["extensions"] = Extensions.ToJToken(version, format);
            }

            return obj;
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