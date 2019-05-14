using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI
{
    /// <summary>
    /// A collection of <see cref="Attachment"/> objects.
    /// </summary>
    //[JsonConverter(typeof(AttachmentCollectionConverter))]
    public class AttachmentCollection : JsonModel, ICollection<Attachment>
    {
        private readonly HashSet<Attachment> _list;

        public int Count => _list.Count;

        public bool IsReadOnly => ((ICollection<Attachment>)_list).IsReadOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentCollection"/> class.
        /// </summary>
        public AttachmentCollection()
        {
            // foreach over List<T> to avoid boxing the Enumerator
            _list = new HashSet<Attachment>();
        }

        public AttachmentCollection(string jsonString) : this(JObject.Parse(jsonString)) { }
        public AttachmentCollection(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public AttachmentCollection(JObject jobj, ApiVersion version)
        {
            var jarr = jobj.Value<JArray>("attachment");
            foreach (var item in jarr)
            {
                Add(new Attachment(item.Value<JObject>(), version));
            }
        }

        public void AddAttachment(Attachment attachment)
        {
            if (!Contains(attachment))
            {
                Add(attachment);
            }
        }

        public Attachment GetAttachment(string sha2)
        {
            if (TryGetAttachment(sha2, out Attachment attachment))
            {
                return attachment;
            }

            return null;
        }

        public bool TryGetAttachment(string sha2, out Attachment attachment)
        {
            attachment = _list.FirstOrDefault(x => x.SHA2 == sha2);
            return attachment != null;
        }

        public void Add(Attachment item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(Attachment item)
        {
            return _list.Any(x=> x.SHA2 == item.SHA2);
        }

        public void CopyTo(Attachment[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(Attachment item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<Attachment> GetEnumerator()
        {
            return ((ICollection<Attachment>)_list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Attachment>)_list).GetEnumerator();
        }

        public override JObject ToJObject(ApiVersion version, ResultFormat format)
        {
            throw new NotImplementedException();
        }
    }
}