using Newtonsoft.Json.Serialization;
using System;

namespace Doctrina.xAPI.Json.Serialization
{
    public class CaseSensitiveJsonObjectContract : JsonObjectContract
    {

        /// <summary>
        /// Gets the object's properties.
        /// </summary>
        /// <value>The object's properties.</value>
        public new CaseSensitiveJsonPropertyCollection Properties { get; }

        public CaseSensitiveJsonObjectContract(Type underlyingType)
            : base(underlyingType)
        {
        }

        public CaseSensitiveJsonObjectContract(JsonObjectContract contract)
            : base(contract.UnderlyingType)
        {
            //MemberSerialization = contract.MemberSerialization;
            //OnSerializingCallbacks.Add(contract.OnSerializingCallbacks);
        }
    }
}
