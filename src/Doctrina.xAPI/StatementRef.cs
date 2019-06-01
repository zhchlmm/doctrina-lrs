using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class StatementRef : StatementObjectBase, IStatementObject
    {
        public StatementRef() { }
        public StatementRef(JsonString jsonString) : this(jsonString.ToJToken()) { }
        public StatementRef(JToken jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public StatementRef(JToken jobj, ApiVersion version)
        {
            if (!AllowObject(jobj))
            {
                return;
            }

            if (DisallowNull(jobj["id"]) && AllowString(jobj["id"]))
            {
                // TODO: Required
                Id = Guid.Parse(jobj.Value<string>("id"));
            }
        }

        protected override ObjectType OBJECT_TYPE => ObjectType.StatementRef;

        /// <summary>
        /// The UUID of a Statement.
        /// </summary>
        public Guid Id { get; set; }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            jobj["id"] = Id.ToString();

            return jobj;
        }

        public override bool Equals(object obj)
        {
            var @ref = obj as StatementRef;
            return @ref != null &&
                   base.Equals(obj) &&
                   Id.Equals(@ref.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + EqualityComparer<Guid>.Default.GetHashCode(Id);
        }

        public static implicit operator StatementRef(JObject jobj)
        {
            return new StatementRef(jobj);
        }

        public static bool operator ==(StatementRef ref1, StatementRef ref2)
        {
            return EqualityComparer<StatementRef>.Default.Equals(ref1, ref2);
        }

        public static bool operator !=(StatementRef ref1, StatementRef ref2)
        {
            return !(ref1 == ref2);
        }
    }
}
