using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public class ActivityCollection : JsonModel<JArray>, ICollection<Activity>
    {
        private readonly ICollection<Activity> activities = new HashSet<Activity>();

        public ActivityCollection() { }
        public ActivityCollection(string jsonString) : this(JToken.Parse(jsonString)) { }
        public ActivityCollection(JToken jtoken) : this(jtoken, ApiVersion.GetLatest()) { }
        public ActivityCollection(JToken jtoken, ApiVersion version)
        {
            if (jtoken.Type == JTokenType.Array)
            {
                var activities = jtoken.Value<JArray>();
                foreach (var actvitiy in activities)
                {
                    Add(new Activity(actvitiy.Value<JObject>(), version));
                }
            }
            else
            {
                Add(new Activity(jtoken.Value<JObject>(), version));
            }
        }

        public int Count => activities.Count;

        public bool IsReadOnly => activities.IsReadOnly;

        public void Add(Activity item)
        {
            activities.Add(item);
        }

        public void Clear()
        {
            activities.Clear();
        }

        public bool Contains(Activity item)
        {
            return activities.Contains(item);
        }

        public void CopyTo(Activity[] array, int arrayIndex)
        {
            activities.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Activity> GetEnumerator()
        {
            return activities.GetEnumerator();
        }

        public bool Remove(Activity item)
        {
            return activities.Remove(item);
        }

        public override JArray ToJToken(ApiVersion version, ResultFormat format)
        {
            var jarr = new JArray();
            foreach (var ac in activities)
            {
                jarr.Add(ac.ToJToken(version, ResultFormat.Ids));
            }
            return jarr;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return activities.GetEnumerator();
        }
    }
}
