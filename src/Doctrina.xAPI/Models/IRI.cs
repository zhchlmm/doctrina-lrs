using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Models
{
    /// <summary>
    /// Internationalized Resource Identifiers
    /// </summary>
    [TypeConverter(typeof(IRITypeConverter))]
    public class Iri
    {
        private readonly string _iriString = null;

        public Iri() { }

        public Iri(string iriString)
        {
            var regex = new Regex("(\b(https?|ftp|file)://)?[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]");
            if (!regex.Match(iriString).Success)
                throw new FormatException($"IRI '{iriString}' not a well formated IRI string.");

            this._iriString = iriString;
        }

        public override string ToString()
        {
            return _iriString;
        }

        public static bool TryParse(string iriString, out Iri iri)
        {
            iri = null;
            try
            {
                iri = new Iri(iriString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static implicit operator Iri(string iriString)
        {
            return new Iri(iriString);
        }
    }

    internal class IRITypeConverter: TypeConverter
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
                Iri iri = null;
                if (Iri.TryParse(value as string, out iri))
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
