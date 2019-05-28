using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Doctrina.xAPI
{

    // NOTE: A SubStatement MUST NOT have the "id", "stored", "version" or "authority" properties.
    /// <summary>
    /// A SubStatement is like a StatementRef in that it is included as part of a containing Statement, but unlike a StatementRef, it does not represent an event that has occurred. 
    /// It can be used to describe, for example, a predication of a potential future Statement or the behavior a teacher looked for when evaluating a student (without representing the student actually doing that behavior)
    /// SubStatement <see cref="https://github.com/adlnet/xAPI-Spec/blob/master/xAPI-Data.md#substatements"/>
    /// </summary>

    [JsonObject]
    public class SubStatement : StatementBase, IStatementObject
    {
        public ObjectType ObjectType => ObjectType.SubStatement;

        public SubStatement() : base() { }
        public SubStatement(JsonString jsonString) : this(jsonString.ToJObject()) { }
        public SubStatement(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public SubStatement(JObject jobj, ApiVersion version) : base(jobj, version)
        {
        }

        public override bool Equals(object obj)
        {
            var statement = obj as SubStatement;
            return statement != null &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = base.ToJToken(version, format);

            return jobj;
        }

        public static implicit operator SubStatement(JObject jobj)
        {
            return new SubStatement(jobj);
        }

        public static bool operator ==(SubStatement statement1, SubStatement statement2)
        {
            return EqualityComparer<SubStatement>.Default.Equals(statement1, statement2);
        }

        public static bool operator !=(SubStatement statement1, SubStatement statement2)
        {
            return !(statement1 == statement2);
        }
    }
}
