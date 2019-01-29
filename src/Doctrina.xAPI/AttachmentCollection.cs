using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Doctrina.xAPI
{
    /// <summary>
    /// A collection of <see cref="Attachment"/> objects.
    /// </summary>
    //[JsonConverter(typeof(AttachmentCollectionConverter))]
    public class AttachmentCollection : KeyedCollection<string, Attachment>
    {
        private readonly List<Attachment> _list;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentCollection"/> class.
        /// </summary>
        public AttachmentCollection()
            : base(StringComparer.Ordinal)
        {
            // foreach over List<T> to avoid boxing the Enumerator
            _list = (List<Attachment>)Items;
        }

        protected override string GetKeyForItem(Attachment item)
        {
            return item.SHA2;
        }

        public void AddAttachment(Attachment attachment)
        {
            if (Contains(attachment.SHA2))
            {

            }
        }

        public Attachment GetAttachment(string sha2)
        {
            if (TryGetValue(sha2, out Attachment attachment))
            {
                return attachment;
            }

            return null;
        }

        public bool TryGetValue(string key, out Attachment item)
        {
            if (Dictionary == null)
            {
                item = default(Attachment);
                return false;
            }

            return Dictionary.TryGetValue(key, out item);
        }
    }
}