using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Doctrina.Domain.Entities.DataTypes
{
    public class LanguageMapCollection : KeyedCollection<string, LanguageMapDataType>
    {
        protected override string GetKeyForItem(LanguageMapDataType item)
        {
            return item.LanguageCode;
        }
    }

    public class LanguageMapDataType
    {
        public string LanguageCode { get; set; }

        public string Description { get; set; }
    }
}
