using xAPI.Core.Schema.Providers;
using xAPI.Core.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.Collections.Generic;

namespace xAPI.Core.Models
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
    }
}
