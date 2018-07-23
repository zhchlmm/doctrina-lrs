using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Persistence.Models
{
    public enum EntityObjectType
    {
        Agent, Group, Activity, SubStatement, StatementRef
    }

    public class ObjectTypeTypeConverter: TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if(sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string)
            {
                if (Enum.TryParse(typeof(EntityObjectType), value as string, out object converted))
                {
                    return (EntityObjectType)converted;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
