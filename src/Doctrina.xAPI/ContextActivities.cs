using Doctrina.xAPI.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonConverter(typeof(ContextActivitiesConverter))]
    [JsonObject]
    public class ContextActivities : JsonModel
    {
        public ContextActivities() { }
        public ContextActivities(string jsonString) : this(JObject.Parse(jsonString)) { }
        public ContextActivities(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }

        public ContextActivities(JObject jobj, ApiVersion version)
        {
            if (jobj["parent"] != null)
            {
                Parent = new ActivityCollection(jobj.Value<JToken>("parent"), version);
            }

            if (jobj["category"] != null)
            {
                Category = new ActivityCollection(jobj.Value<JToken>("category"), version);
            }

            if (jobj["grouping"] != null)
            {
                Grouping = new ActivityCollection(jobj.Value<JToken>("grouping"), version);
            }

            if (jobj["other"] != null)
            {
                Other = new ActivityCollection(jobj.Value<JToken>("other"), version);
            }
        }

        /// <summary>
        /// Parent: an Activity with a direct relation to the Activity which is the Object of the Statement. 
        /// In almost all cases there is only one sensible parent or none, not multiple. 
        /// For example: a Statement about a quiz question would have the quiz as its parent Activity.
        /// </summary>
        [JsonProperty("parent",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public ActivityCollection Parent { get; set; }

        /// <summary>
        /// Category: an Activity used to categorize the Statement. "Tags" would be a synonym. 
        /// Category SHOULD be used to indicate a profile of xAPI behaviors, as well as other categorizations. 
        /// For example: Anna attempts a biology exam, and the Statement is tracked using the cmi5 profile. 
        /// The Statement's Activity refers to the exam, and the category is the cmi5 profile.
        /// </summary>
        [JsonProperty("category",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public ActivityCollection Category { get; set; }

        /// <summary>
        /// Grouping: an Activity with an indirect relation to the Activity which is the Object of the Statement. 
        /// For example: a course that is part of a qualification. The course has several classes. 
        /// The course relates to a class as the parent, the qualification relates to the class as the grouping.
        /// </summary>
        [JsonProperty("grouping",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public ActivityCollection Grouping { get; set; }

        /// <summary>
        /// Other: a contextActivity that doesn't fit one of the other properties. 
        /// For example: Anna studies a textbook for a biology exam. The Statement's Activity refers to the textbook, and the exam is a contextActivity of type other.
        /// </summary>
        [JsonProperty("other",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public ActivityCollection Other { get; set; }

        public override bool Equals(object obj)
        {
            var activities = obj as ContextActivities;
            return activities != null &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Parent, activities.Parent) &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Category, activities.Category) &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Grouping, activities.Grouping) &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Other, activities.Other);
        }

        public override int GetHashCode()
        {
            var hashCode = -682935721;
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Grouping);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Other);
            return hashCode;
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();
            if(Parent != null)
            {
                jobj["parent"] = Parent.ToJToken(version, format);
            }
            if (Category != null)
            {
                jobj["category"] = Category.ToJToken(version, format);
            }
            if (Grouping != null)
            {
                jobj["grouping"] = Grouping.ToJToken(version, format);
            }
            if (Other != null)
            {
                jobj["other"] = Other.ToJToken(version, format);
            }
            return jobj;
        }

        public static bool operator ==(ContextActivities activities1, ContextActivities activities2)
        {
            return EqualityComparer<ContextActivities>.Default.Equals(activities1, activities2);
        }

        public static bool operator !=(ContextActivities activities1, ContextActivities activities2)
        {
            return !(activities1 == activities2);
        }
    }
}
