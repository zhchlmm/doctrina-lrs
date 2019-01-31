using Doctrina.Domain.Entities.DataTypes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Doctrina.Persistence.ValueConverters
{
    public class ExtensionsValueConverter : ValueConverter<ExtensionsDataType, string>
    {
        public ExtensionsValueConverter(ConverterMappingHints mappingHints = null)
            : base(covertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }

        private static Expression<Func<ExtensionsDataType, string>> covertToProviderExpression = e => ToDataStore(e);
        private static Expression<Func<string, ExtensionsDataType>> convertFromProviderExpression = e => FromDataStore(e);

        public static string ToDataStore(ExtensionsDataType extensions)
        {
            return JsonConvert.SerializeObject(extensions);
        }

        public static ExtensionsDataType FromDataStore(string strExtesions)
        {
            return JsonConvert.DeserializeObject<ExtensionsDataType>(strExtesions);
        }
    }
}
