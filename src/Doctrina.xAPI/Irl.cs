using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doctrina.xAPI
{
    /// <summary>
    /// Internationalized Resource Identifiers
    /// </summary>
    [Obsolete("Use URIs instead.")]
    [TypeConverter(typeof(IrlTypeConverter))]
    public class Irl
    {
        private readonly string _irlString = null;

        public Irl() { }

        public Irl(string irlString)
        {
            if(!Uri.IsWellFormedUriString(irlString, UriKind.RelativeOrAbsolute))
                throw new FormatException($"IRL '{irlString}' is not a well formatted IRL string.");

            _irlString = irlString;
        }

        public override string ToString()
        {
            return _irlString;
        }

        public static bool TryParse(string iriString, out Irl iri)
        {
            iri = null;
            try
            {
                iri = new Irl(iriString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            var iri = obj as Irl;
            return iri != null &&
                   _irlString == iri._irlString;
        }

        public override int GetHashCode()
        {
            return 314876093 + EqualityComparer<string>.Default.GetHashCode(_irlString);
        }

        public static bool operator ==(Irl iri1, Irl iri2)
        {
            return EqualityComparer<Irl>.Default.Equals(iri1, iri2);
        }

        public static bool operator !=(Irl iri1, Irl iri2)
        {
            return !(iri1 == iri2);
        }

        public static implicit operator Uri(Irl d)
        {
            return new Uri(d.ToString());
        }

        public static implicit operator Irl(Uri d)
        {
            return new Irl(d.ToString());
        }
    }

    internal class IrlTypeConverter : TypeConverter
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
                Irl iri = null;
                if (Irl.TryParse(value as string, out iri))
                {
                    return iri;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        //public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        //{
        //    if(destinationType == typeof(IRI))
        //    {
        //        return 
        //    }

        //    return base.CanConvertTo(context, destinationType);
        //}
    }
}
