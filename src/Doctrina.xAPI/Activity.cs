using Doctrina.xAPI.InteractionTypes;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public class Activity : StatementObjectBase, IObjectType
    {
        public Activity() { }

        public Activity(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public Activity(JObject jobj, ApiVersion version)
            : base(jobj, version)
        {
            if (jobj["id"] != null)
            {
                Id = jobj.Value<Iri>("id");
            }

            if (jobj["definition"] != null)
            {
                var jdefinition = jobj.Value<JObject>("definition");

                JToken tokenInteractionType = jdefinition["interactionType"];
                if (tokenInteractionType != null)
                {
                    InteractionType interactionType = tokenInteractionType.Value<string>();
                    Definition = interactionType.CreateInstance(jdefinition, version);
                }
                else
                {
                    Definition = new ActivityDefinition(jobj.Value<JObject>("definition"), version);
                }
            }
        }

        protected override ObjectType OBJECT_TYPE => ObjectType.Activity;

        /// <summary>
        /// 
        /// </summary>
        public Iri Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ActivityDefinition Definition { get; set; }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var result = base.ToJToken(version, format);

            if (Id != null)
            {
                result.Add("id", Id.ToString());
            }

            if (Definition != null)
            {
                result.Add("definition", Definition.ToJToken(version, format));
            }

            return result;
        }

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
