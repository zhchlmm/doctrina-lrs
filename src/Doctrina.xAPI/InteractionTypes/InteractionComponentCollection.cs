using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Doctrina.xAPI.InteractionTypes
{
    public class InteractionComponentCollection : JsonModel<JArray>, ICollection<InteractionComponent>
    {
        public ICollection<InteractionComponent> Components = new HashSet<InteractionComponent>();

        public InteractionComponentCollection()
        {
        }

        public InteractionComponentCollection(string jsonString)
            : this(JArray.Parse(jsonString))
        {
        }

        public InteractionComponentCollection(JArray jarr)
           : this(jarr, ApiVersion.GetLatest())
        {
        }

        public InteractionComponentCollection(JArray jarr, ApiVersion version)
        {
            foreach(var item in jarr)
            {
                Add(new InteractionComponent(item.Value<JObject>(), version));
            }
        }

        public int Count => Components.Count;

        public bool IsReadOnly => Components.IsReadOnly;

        public void Add(InteractionComponent item)
        {
            Components.Add(item);
        }

        public void Clear()
        {
            Components.Clear();
        }

        public bool Contains(InteractionComponent item)
        {
            return Components.Contains(item);
        }

        public void CopyTo(InteractionComponent[] array, int arrayIndex)
        {
            Components.CopyTo(array, arrayIndex);
        }

        public IEnumerator<InteractionComponent> GetEnumerator()
        {
            return Components.GetEnumerator();
        }

        public bool Remove(InteractionComponent item)
        {
            return Components.Remove(item);
        }

        public override JArray ToJToken(ApiVersion version, ResultFormat format)
        {
            var jarr = new JArray();
            foreach(var component in Components)
            {
                jarr.Add(component.ToJToken(vers));
            }
            return jarr;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Components.GetEnumerator();
        }
    }
}
