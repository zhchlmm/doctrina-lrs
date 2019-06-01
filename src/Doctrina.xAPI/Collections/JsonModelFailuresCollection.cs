using Doctrina.xAPI.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Collections
{
    public class JsonModelFailuresCollection : ICollection<JsonModelFailure>
    {
        private readonly ICollection<JsonModelFailure> Failures = new HashSet<JsonModelFailure>();

        public JsonModelFailuresCollection() { }

        public JsonModelFailuresCollection(ICollection<JsonModelFailure> failures)
        {
            Failures = failures;
        }

        public int Count => Failures.Count;

        public bool IsReadOnly => Failures.IsReadOnly;

        public void Add(string name, string message)
        {
            Failures.Add(new JsonModelFailure(name, message));
        }

        public void Add(JsonModelFailure item)
        {
            Failures.Add(item);
        }

        public void Clear()
        {
            Failures.Clear();
        }

        public bool Contains(JsonModelFailure item)
        {
            return Failures.Contains(item);
        }

        public void CopyTo(JsonModelFailure[] array, int arrayIndex)
        {
            Failures.CopyTo(array, arrayIndex);
        }

        public IEnumerator<JsonModelFailure> GetEnumerator()
        {
            return Failures.GetEnumerator();
        }

        public bool Remove(JsonModelFailure item)
        {
            return Failures.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Failures.GetEnumerator();
        }
    }
}
