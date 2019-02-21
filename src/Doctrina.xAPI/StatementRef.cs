using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI
{
    [JsonObject]
    public class StatementRef : StatementObjectBase
    {
        protected override ObjectType OBJECT_TYPE => ObjectType.StatementRef;

        /// <summary>
        /// The UUID of a Statement.
        /// </summary>
        [JsonProperty("id",
            Required = Required.Always)]
        public Guid Id { get; set; }

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
