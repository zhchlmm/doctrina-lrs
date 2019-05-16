using Doctrina.Domain.Entities.OwnedTypes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Linq.Expressions;

namespace Doctrina.Persistence.ValueConverters
{
    public class InteractionComponentCollectionValueConverter : ValueConverter<ExtensionEntity, string>
    {
        public InteractionComponentCollectionValueConverter()
            : base(convertToProviderExpression, convertFromProviderExpression)
        {
        }

        private static Expression<Func<ExtensionEntity, string>> convertToProviderExpression = e => ToDataStore(e);
        private static Expression<Func<string, ExtensionEntity>> convertFromProviderExpression = e => FromDataStore(e);

        private static string ToDataStore(ExtensionEntity e)
        {
            return JsonConvert.SerializeObject(e);
        }


        private static ExtensionEntity FromDataStore(string e)
        {
            return JsonConvert.DeserializeObject<ExtensionEntity>(e);
        }
    }
}
