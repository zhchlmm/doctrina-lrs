using Doctrina.Domain.Entities.DataTypes;
using System.Collections.ObjectModel;

namespace Doctrina.Domain.Entities
{
    public class InteractionComponent
    {
        public string Id { get; set; }

        public LanguageMapCollection Description { get; set; }
    }

    public class InteractionComponentCollection : KeyedCollection<string, InteractionComponent>
    {
        protected override string GetKeyForItem(InteractionComponent item)
        {
            return item.Id;
        }
    }
}
