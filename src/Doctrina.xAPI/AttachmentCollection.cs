using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Doctrina.xAPI
{
    /// <summary>
    /// A collection of <see cref="Attachment"/> objects.
    /// </summary>
    //[JsonConverter(typeof(AttachmentCollectionConverter))]
    public class AttachmentCollection : JsonModel<JArray>, ICollection<Attachment>
    {
        private readonly HashSet<Attachment> Attachments;

        public int Count => Attachments.Count;

        public bool IsReadOnly => ((ICollection<Attachment>)Attachments).IsReadOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentCollection"/> class.
        /// </summary>
        public AttachmentCollection()
        {
            // foreach over List<T> to avoid boxing the Enumerator
            Attachments = new HashSet<Attachment>();
        }

        public AttachmentCollection(JsonString jsonString) : this(jsonString.ToJArray()) { }
        public AttachmentCollection(JArray jarr) : this(jarr, ApiVersion.GetLatest()) { }
        public AttachmentCollection(JArray jarr, ApiVersion version)
        {
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
            attachment = Attachments.FirstOrDefault(x => x.SHA2 == sha2);
            return attachment != null;
        }

        public void Add(Attachment item)
        {
            Attachments.Add(item);
        }

        public void Clear()
        {
            Attachments.Clear();
        }

        public bool Contains(Attachment item)
        {
            return Attachments.Any(x => x.SHA2 == item.SHA2);
        }

        public void CopyTo(Attachment[] array, int arrayIndex)
        {
            Attachments.CopyTo(array, arrayIndex);
        }

        public bool Remove(Attachment item)
        {
            return Attachments.Remove(item);
        }

        public IEnumerator<Attachment> GetEnumerator()
        {
            return ((ICollection<Attachment>)Attachments).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<Attachment>)Attachments).GetEnumerator();
        }

        public override JArray ToJToken(ApiVersion version, ResultFormat format)
        {
            if(Attachments.Count == 0)
            {
                return null;
            }

            var jArray = new JArray();

            foreach (var attachment in Attachments)
            {
                jArray.Add(attachment.ToJToken(version, format));
            }

            return jArray;
        }
    }
}