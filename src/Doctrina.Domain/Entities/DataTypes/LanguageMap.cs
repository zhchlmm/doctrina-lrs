using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Doctrina.Domain.Entities.DataTypes
{
    public class LanguageMapCollection : KeyedCollection<string, LanguageMap>
    {
        protected override string GetKeyForItem(LanguageMap item)
        {
            return item.LanguageCode;
        }
    }

    public class LanguageMap
    {
        public string LanguageCode { get; set; }

        public string Description { get; set; }
    }
}
