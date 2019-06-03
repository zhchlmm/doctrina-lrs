using Doctrina.xAPI.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Collections
{
    public class JsonModelErrorsCollection : ICollection<JsonModelError>
    {
        private readonly ICollection<JsonModelError> Errors = new HashSet<JsonModelError>();

        public JsonModelErrorsCollection() { }

        public JsonModelErrorsCollection(ICollection<JsonModelError> errors)
        {
            Errors = errors;
        }

        public int Count => Errors.Count;

        public bool IsReadOnly => Errors.IsReadOnly;

        public void Add(string name, string message)
        {
            Errors.Add(new JsonModelError(name, message));
        }

        public void Add(JsonModelError item)
        {
            Errors.Add(item);
        }

        public void Clear()
        {
            Errors.Clear();
        }

        public bool Contains(JsonModelError item)
        {
            return Errors.Contains(item);
        }

        public void CopyTo(JsonModelError[] array, int arrayIndex)
        {
            Errors.CopyTo(array, arrayIndex);
        }

        public IEnumerator<JsonModelError> GetEnumerator()
        {
            return Errors.GetEnumerator();
        }

        public bool Remove(JsonModelError item)
        {
            return Errors.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Errors.GetEnumerator();
        }
    }
}
