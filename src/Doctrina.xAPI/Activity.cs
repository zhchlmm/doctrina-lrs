using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class Activity : StatementObjectBase
    {
        protected override ObjectType OBJECT_TYPE => ObjectType.Activity;

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("id",
            Required = Required.Always)]
        public Iri Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("definition", 
            Required = Required.DisallowNull,
            NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ActivityDefinitionConverter))]
        public ActivityDefinition Definition { get; set; }

        public override bool Equals(object obj)
        {
            var activity = obj as Activity;
            return activity != null &&
                   base.Equals(obj) &&
                   EqualityComparer<Iri>.Default.Equals(Id, activity.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Iri>.Default.GetHashCode(Id);
        }

        public static bool operator ==(Activity activity1, Activity activity2)
        {
            return EqualityComparer<Activity>.Default.Equals(activity1, activity2);
        }

        public static bool operator !=(Activity activity1, Activity activity2)
        {
            return !(activity1 == activity2);
        }
    }
}
