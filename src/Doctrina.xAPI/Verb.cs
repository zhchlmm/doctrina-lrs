using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public class Verb : JsonModel, IVerb
    {
        /// <summary>
        /// Corresponds to a Verb definition. (Required)
        /// Each Verb definition corresponds to the meaning of a Verb, not the word. 
        /// </summary>
        [JsonProperty("id",
            Required = Required.Always)]
        public Iri Id { get; set; }

        [JsonProperty("display",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public LanguageMap Display { get; set; }

        public override bool Equals(object obj)
        {
            var verb = obj as Verb;
            return verb != null &&
                   EqualityComparer<Iri>.Default.Equals(Id, verb.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Iri>.Default.GetHashCode(Id);
        }

        public override JObject ToJObject(ApiVersion version, ResultFormat format)
        {
            var obj = new JObject
            {
                ["id"] = Id.ToString(),
                ["display"] = Display.ToJObject(version, format)
            };
            return obj;
        }

        public static bool operator ==(Verb verb1, Verb verb2)
        {
            return EqualityComparer<Verb>.Default.Equals(verb1, verb2);
        }

        public static bool operator !=(Verb verb1, Verb verb2)
        {
            return !(verb1 == verb2);
        }
    }
}