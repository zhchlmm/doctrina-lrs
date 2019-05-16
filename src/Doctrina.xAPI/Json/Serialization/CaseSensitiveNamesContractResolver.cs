using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Doctrina.xAPI.Json.Serialization
{
    public class CaseSensitiveNamesContractResolver : DefaultContractResolver
    {
        private readonly DefaultJsonNameTable _nameTable = new DefaultJsonNameTable();

        public CaseSensitiveNamesContractResolver()
        {
            //NamingStrategy = new CamelCaseNamingStrategy
            //{
            //    OverrideSpecifiedNames = false
            //};
        }

        //protected override JsonObjectContract CreateObjectContract(Type objectType)
        //{
        //    var contract = base.CreateObjectContract(objectType);
        //    var newContract = new CaseSensitiveJsonObjectContract(contract);
        //    return newContract;
        //}


        /// <summary>
        /// Creates properties for the given <see cref="JsonContract"/>.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>Properties for the given <see cref="JsonContract"/>.</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            List<MemberInfo> members = GetSerializableMembers(type);
            if (members == null)
            {
                throw new JsonSerializationException("Null collection of serializable members returned.");
            }

            DefaultJsonNameTable nameTable = GetNameTable();

            CaseSensitiveJsonPropertyCollection properties = new CaseSensitiveJsonPropertyCollection(type);

            foreach (MemberInfo member in members)
            {
                JsonProperty property = CreateProperty(member, memberSerialization);

                if (property != null)
                {
                    // nametable is not thread-safe for multiple writers
                    lock (nameTable)
                    {
                        property.PropertyName = nameTable.Add(property.PropertyName);
                    }

                    properties.AddProperty(property);
                }
            }

            IList<JsonProperty> orderedProperties = properties.OrderBy(p => p.Order ?? -1).ToList();
            return orderedProperties;
        }

        internal virtual DefaultJsonNameTable GetNameTable()
        {
            return _nameTable;
        }
    }

}
