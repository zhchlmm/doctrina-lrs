using Doctrina.Domain.Entities.DataTypes;
using System;

namespace Doctrina.Domain.Entities
{
    public class VerbEntity
    {
        public Guid Key { get; set; }

        public string Id { get; set; }

        public LanguageMapDataType Display { get; set; }
    }
}
