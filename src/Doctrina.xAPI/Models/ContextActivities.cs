using Doctrina.xAPI.Json.Converters;
using Doctrina.xAPI.Schema.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema.Generation;
using System.Collections.Generic;

namespace Doctrina.xAPI.Models
{
    [JsonConverter(typeof(ContextActivitiesConverter))]
    [JSchemaGenerationProvider(typeof(ContextActivitiesSchemaProvider))]
    [JsonObject]
    public class ContextActivities
    {
        /// <summary>
        /// Parent: an Activity with a direct relation to the Activity which is the Object of the Statement. 
        /// In almost all cases there is only one sensible parent or none, not multiple. 
        /// For example: a Statement about a quiz question would have the quiz as its parent Activity.
        /// </summary>
        [JsonProperty("parent",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Activity[] Parent { get; set; }

        /// <summary>
        /// Category: an Activity used to categorize the Statement. "Tags" would be a synonym. 
        /// Category SHOULD be used to indicate a profile of xAPI behaviors, as well as other categorizations. 
        /// For example: Anna attempts a biology exam, and the Statement is tracked using the cmi5 profile. 
        /// The Statement's Activity refers to the exam, and the category is the cmi5 profile.
        /// </summary>
        [JsonProperty("category",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Activity[] Category { get; set; }

        /// <summary>
        /// Grouping: an Activity with an indirect relation to the Activity which is the Object of the Statement. 
        /// For example: a course that is part of a qualification. The course has several classes. 
        /// The course relates to a class as the parent, the qualification relates to the class as the grouping.
        /// </summary>
        [JsonProperty("grouping",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Activity[] Grouping { get; set; }

        /// <summary>
        /// Other: a contextActivity that doesn't fit one of the other properties. 
        /// For example: Anna studies a textbook for a biology exam. The Statement's Activity refers to the textbook, and the exam is a contextActivity of type other.
        /// </summary>
        [JsonProperty("other",
            NullValueHandling = NullValueHandling.Ignore,
            Required = Required.DisallowNull)]
        public Activity[] Other { get; set; }

        public override bool Equals(object obj)
        {
            var activities = obj as ContextActivities;
            return activities != null &&
                   EqualityComparer<Activity[]>.Default.Equals(Parent, activities.Parent) &&
                   EqualityComparer<Activity[]>.Default.Equals(Category, activities.Category) &&
                   EqualityComparer<Activity[]>.Default.Equals(Grouping, activities.Grouping) &&
                   EqualityComparer<Activity[]>.Default.Equals(Other, activities.Other);
        }

        public override int GetHashCode()
        {
            var hashCode = -682935721;
            hashCode = hashCode * -1521134295 + EqualityComparer<Activity[]>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<Activity[]>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<Activity[]>.Default.GetHashCode(Grouping);
            hashCode = hashCode * -1521134295 + EqualityComparer<Activity[]>.Default.GetHashCode(Other);
            return hashCode;
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
