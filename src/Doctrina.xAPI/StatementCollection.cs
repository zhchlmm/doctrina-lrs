using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Doctrina.xAPI
{
    /// <summary>
    /// A collection of <see cref="Statement"/> objects.
    /// </summary>
    public class StatementCollection : KeyedCollection<Guid, Statement>
    {
        private readonly List<Statement> _list;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatementCollection"/> class.
        /// </summary>
        public StatementCollection()
            : base()
        {
            // foreach over List<T> to avoid boxing the Enumerator
            _list = (List<Statement>)Items;
        }

        protected override Guid GetKeyForItem(Statement item)
        {
            return item.Id.Value;
        }

        public bool TryGetValue(Guid key, out Statement item)
        {
            if (Dictionary == null)
            {
                item = default(Statement);
                return false;
            }

            return Dictionary.TryGetValue(key, out item);
        }
    }
}