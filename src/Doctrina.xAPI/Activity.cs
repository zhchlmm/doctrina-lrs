using Doctrina.xAPI.InteractionTypes;
using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class Activity : StatementObjectBase, IStatementTarget
    {
        public Activity() { }

        public Activity(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public Activity(JObject jobj, ApiVersion version)
        {
            if (jobj["id"] != null)
            {
                Id = jobj.Value<Iri>("id");
            }

            if (jobj["definition"] != null)
            {
                var jdefinition = jobj.Value<JObject>("definition");

                JToken tokenInteractionType = jdefinition["interactionType"];
                if(tokenInteractionType != null)
                {
                    InteractionType type = tokenInteractionType.Value<string>();
                    if(type == InteractionType.Choice)
                    {
                        Definition = new Choice(jdefinition, version);
                    }
                    else if (type == InteractionType.FillIn)
                    {
                        Definition = new FillIn(jdefinition, version);
                    }
                    else if (type == InteractionType.LongFillIn)
                    {
                        Definition = new LongFillIn(jdefinition, version);
                    }
                    else if (type == InteractionType.Likert)
                    {
                        Definition = new Likert(jdefinition, version);
                    }
                    else if (type == InteractionType.Matching)
                    {
                        Definition = new Matching(jdefinition, version);
                    }
                    else if (type == InteractionType.Numeric)
                    {
                        Definition = new Numeric(jdefinition, version);
                    }
                    else if (type == InteractionType.Sequencing)
                    {
                        Definition = new Performance(jdefinition, version);
                    }
                    else if (type == InteractionType.Performance)
                    {
                        Definition = new Performance(jdefinition, version);
                    }
                    else if (type == InteractionType.Other)
                    {
                        Definition = new Other(jdefinition, version);
                    }

                    throw new NotImplementedException();
                }

                Definition = new ActivityDefinition(jobj.Value<JObject>("definition"), version);
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

        public override JObject ToJObject(ApiVersion version, ResultFormat format)
        {
            var result = base.ToJObject(version, format);

            if (Id != null)
            {
                result.Add("id", Id.ToString());
            }

            if (Definition != null)
            {
                result.Add("definition", Definition.ToJObject(version, format));
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
