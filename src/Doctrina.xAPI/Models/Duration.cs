using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Doctrina.xAPI.Models
{
    // TODO: Implement custom duration
    /// <summary>
    /// ISO 8601 Duration
    /// </summary>
    [TypeConverter(typeof(DurationTypeConverter))]
    public class Duration
    {
        private readonly string _durationString = null;

        public Duration() { }

        public Duration(string durationString)
        {
            try
            {
                var parsedDuration = XmlConvert.ToTimeSpan(durationString);
            }
            catch (Exception)
            {
                throw new FormatException($"'{durationString}' is not a well formatted ISO 8601 Duration string.");
            }

            this._durationString = durationString;
        }

        public override string ToString()
        {
            return _durationString;
        }

        public static bool TryParse(string durationString, out Duration iri)
        {
            iri = null;
            try
            {
                iri = new Duration(durationString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Equals(Uri obj)
        {
            return _durationString == obj.ToString();
        }
    }

    internal class DurationTypeConverter: TypeConverter
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
                Duration duration = null;
                if (Duration.TryParse(value as string, out duration))
                {
                    return duration;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(TimeSpan))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }
    }
}
