using Doctrina.Domain.Entities.DataTypes;

namespace Doctrina.Domain.Entities
{
    public class ActivityDefinitionEntity
    {
        public LanguageMapCollection Name { get; set; }

        public LanguageMapCollection Description { get; set; }

        public string Type { get; set; }

        public string MoreInfo { get; set; }

        public ExtensionsDataType Extensions { get; set; }
    }
}
