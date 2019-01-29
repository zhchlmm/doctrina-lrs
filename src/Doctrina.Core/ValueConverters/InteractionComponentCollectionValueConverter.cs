using Doctrina.Domain.Entities.DataTypes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Doctrina.Persistence.ValueConverters
{
    public class InteractionComponentCollectionValueConverter : ValueConverter<ExtensionsDataType, string>
    {
        public InteractionComponentCollectionValueConverter()
            :base(convertToProviderExpression, convertFromProviderExpression)
        {
        }

        private static Expression<Func<ExtensionsDataType, string>> convertToProviderExpression = e=> ToDataStore(e);
        private static Expression<Func<string, ExtensionsDataType>> convertFromProviderExpression = e=> FromDataStore(e);

        private static string ToDataStore(ExtensionsDataType e)
        {
            return JsonConvert.SerializeObject(e);
        }


        private static ExtensionsDataType FromDataStore(string e)
        {
            return JsonConvert.DeserializeObject<ExtensionsDataType>(e);
        }
    }
}
