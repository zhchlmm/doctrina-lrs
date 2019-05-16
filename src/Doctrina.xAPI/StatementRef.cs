﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public StatementRef() { }
        public StatementRef(string jsonString) : this(JObject.Parse(jsonString)){ }
        public StatementRef(JObject jobj) : this(jobj, ApiVersion.GetLatest()) { }
        public StatementRef(JObject jobj, ApiVersion version)
        {
            Id = jobj.Value<Guid>();
        }

        protected override ObjectType OBJECT_TYPE => ObjectType.StatementRef;

        /// <summary>
        /// The UUID of a Statement.
        /// </summary>
        public Guid Id { get; set; }

        public override JObject ToJToken(ApiVersion version, ResultFormat format)
        {
            return new JObject
            {
                ["id"] = Id.ToString()
            };
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
