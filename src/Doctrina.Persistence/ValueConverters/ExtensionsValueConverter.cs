using Doctrina.Domain.Entities.OwnedTypes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Doctrina.Persistence.ValueConverters
{
    public class ExtensionsValueConverter : ValueConverter<ExtensionEntity, string>
    {
        public ExtensionsValueConverter(ConverterMappingHints mappingHints = null)
            : base(covertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }

        private static Expression<Func<ExtensionEntity, string>> covertToProviderExpression = e => ToDataStore(e);
        private static Expression<Func<string, ExtensionEntity>> convertFromProviderExpression = e => FromDataStore(e);

        public static string ToDataStore(ExtensionEntity extensions)
        {
            return JsonConvert.SerializeObject(extensions);
        }

        public static ExtensionEntity FromDataStore(string strExtesions)
        {
            return JsonConvert.DeserializeObject<ExtensionEntity>(strExtesions);
        }
    }
}
