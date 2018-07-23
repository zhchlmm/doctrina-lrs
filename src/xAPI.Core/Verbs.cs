using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core
{
    public class Verbs
    {
        /// <summary>
        /// Ends the formal tracking of learning content, any statements time stamped after a statement with a terminated verb are not formally tracked.
        /// </summary>
        public const string Terminated = "http://adlnet.gov/expapi/verbs/terminated";

        /// <summary>
        /// Indicates that the actor has completed the object.
        /// </summary>
        public const string Completed = "http://activitystrea.ms/schema/1.0/complete";

        /// <summary>
        /// Used to leave an activity attempt with no intention of returning with the learner progress intact. The expectation is learner progress will be cleared. Should appear immediately before a statement with terminated. A statement with EITHER exited OR suspended should be used before one with terminated. Lack of the two implies the same as exited.
        /// </summary>
        public const string Exited = "http://adlnet.gov/expapi/verbs/exited";

        /// <summary>
        /// Used to suspend an activity with the intention of returning to it later, but not losing progress. Should appear immediately before a statement with terminated. A statement with EITHER exited OR suspended should be used before one with terminated. Lack of the two implies the same as exited. Beginning the suspended activity will always result in a resumed activity.
        /// </summary>
        public const string Suspended = "http://adlnet.gov/expapi/verbs/suspended";

        /// <summary>
        /// A catch-all verb to say that someone viewed, listened to, read, etc. some form of content. There is no assumption of completion or success.
        /// </summary>
        public const string Experienced = "http://adlnet.gov/expapi/verbs/experienced";

        /// <summary>
        /// Learner did not perform the activity to a level of pre-determined satisfaction. Used to affirm the lack of success a learner experienced within the learning content in relation to a threshold. If the user performed below the minimum to the level of this threshold, the content is 'failed'. The opposite of 'passed'.
        /// </summary>
        public const string Failed = "http://adlnet.gov/expapi/verbs/failed";

        /// <summary>
        /// Begins the formal tracking of learning content, any statements time stamped before a statement with an initialized verb are not formally tracked.
        /// </summary>
        public const string Initialized = "http://adlnet.gov/expapi/verbs/initialized";

        /// <summary>
        /// Starts the process of launching the next piece of learning content. There is no expectation if this is done by user or system and no expectation that the learning content is a "SCO". It is highly recommended that the display is used to mirror the behavior. If an activity is launched from another, then launched from may be better. If the activity is launched and then the statement is generated, launched or launched into may be more appropriate.
        /// </summary>
        public const string Launched = "http://adlnet.gov/expapi/verbs/launched";

        /// <summary>
        /// Used to affirm the success a learner experienced within the learning content in relation to a threshold. If the user performed at a minimum to the level of this threshold, the content is 'passed'. The opposite of 'failed'.
        /// </summary>
        public const string Passed = "http://adlnet.gov/expapi/verbs/passed";

        /// <summary>
        /// A value, typically within a scale of progression, to how much of an activity has been accomplished. This is not to be confused with 'mastered', as the level of success or competency a user gained is not guaranteed by progress.
        /// </summary>
        public const string Progressed = "http://adlnet.gov/expapi/verbs/progressed";

        /// <summary>
        /// Used to respond to a question. It could be either the actual answer to a question asked of the actor OR the correctness of an answer to a question asked of the actor. Must follow a statement with asked or another statement with a responded (the top statement with responded) must follow the "asking" statement. The response to the question can be the actual text (usually) response or the correctness of that response. For example, Andy responded to quiz question 1 with result 'response=4' and Andy responded to quiz question 1 with result success=true'. Typically both types of responded statements would follow a single question/interacton.
        /// </summary>
        public const string Responded = "http://adlnet.gov/expapi/verbs/responded";

        /// <summary>
        /// Used to resume suspended attempts on an activity. Should immediately follow a statement with initialized if the attempt is indeed to be resumed. The absence of a resumed statement implies a fresh attempt on the activity. Can only be used on an activity that used a suspended statement.
        /// </summary>
        public const string Resumed = "http://adlnet.gov/expapi/verbs/resumed";

        /// <summary>
        /// A special LRS-reserved verb. Used by the LRS to declare that an activity statement is to be voided from record.
        /// </summary>
        public const string Voided = "http://adlnet.gov/expapi/verbs/voided";

    }
}
