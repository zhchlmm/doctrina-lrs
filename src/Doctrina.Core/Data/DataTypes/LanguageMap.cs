using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Data.DataTypes
{
    [TypeConverter(typeof(LanguageMapTypeConverter))]
    public class LanguageMapDataType
    {
        public static bool TryParse(string s, out LanguageMapDataType langMap)
        {
            langMap = null;
            try
            {
                langMap = JsonConvert.DeserializeObject<LanguageMapDataType>(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class LanguageMapTypeConverter : TypeConverter
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
                if(LanguageMapDataType.TryParse(value as string, out LanguageMapDataType langMap))
                {
                    return langMap;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
