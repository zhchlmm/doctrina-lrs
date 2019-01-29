using Doctrina.Domain.Entities.DataTypes;

namespace Doctrina.Domain.Entities
{
    public class ActivityDefinitionEntity
    {
        public LanguageMapDataType Name { get; set; }

        public LanguageMapDataType Description { get; set; }

        public string Type { get; set; }

        public string MoreInfo { get; set; }

        public DataTypes.ExtensionsDataType Extensions { get; set; }
    }
}
