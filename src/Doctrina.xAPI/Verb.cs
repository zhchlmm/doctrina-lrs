using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public class Verb : JsonModel, IVerb
    {
        public Verb() { }
        public Verb(JsonString jsonString) : this(jsonString.ToJObject())
        {
        }
        public Verb(JObject jObject) : this(jObject, ApiVersion.GetLatest())
        {
        }
        public Verb(JObject jobj, ApiVersion version)
        {
            if (jobj["id"] != null)
            {
                Id = new Iri(jobj.Value<string>("id"));
            }
        }

        /// <summary>
        /// Corresponds to a Verb definition. (Required)
        /// Each Verb definition corresponds to the meaning of a Verb, not the word. 
        /// </summary>
        public Iri Id { get; set; }

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

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject
            {
                ["id"] = Id.ToString(),
            };

            if (Display != null)
            {
                jobj["display"] = Display.ToJToken(version, format);
            }

            return jobj;
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