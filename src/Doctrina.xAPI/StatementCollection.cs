using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    /// <summary>
    /// A collection of <see cref="Statement"/> objects.
    /// </summary>
    public class StatementCollection : JsonModel<JArray>, ICollection<Statement>
    {
        private readonly ICollection<Statement> Statements = new HashSet<Statement>();

        public StatementCollection() { }
        public StatementCollection(ICollection<Statement> statements)
        {
            Statements = statements;
        }
        public StatementCollection(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public StatementCollection(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public StatementCollection(JToken jobj, ApiVersion version)
        {
            foreach (var item in jobj)
            {
                Add(new Statement(item, version));
            }
        }

        public int Count => Statements.Count;

        public bool IsReadOnly => Statements.IsReadOnly;

        public void Add(Statement item)
        {
            Statements.Add(item);
        }

        public void Clear()
        {
            Statements.Clear();
        }

        public bool Contains(Statement item)
        {
            return Statements.Contains(item);
        }

        public void CopyTo(Statement[] array, int arrayIndex)
        {
            Statements.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Statement> GetEnumerator()
        {
            return Statements.GetEnumerator();
        }

        public bool Remove(Statement item)
        {
            return Statements.Remove(item);
        }

        public override JArray ToJToken(ApiVersion version, ResultFormat format)
        {
            var jarr = new JArray();
            foreach(var stmt in Statements)
            {
                jarr.Add(stmt.ToJToken(version, format));
            }
            return jarr;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Statements.GetEnumerator();
        }
    }
}