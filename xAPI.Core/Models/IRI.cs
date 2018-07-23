using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models
{
    /// <summary>
    /// Internationalized Resource Identifiers
    /// </summary>
    [TypeConverter(typeof(IRITypeConverter))]
    public class IRI
    {
        private readonly string _iriString = null;

        public IRI() { }

        public IRI(string iriString)
        {
            if (!Uri.IsWellFormedUriString(iriString, UriKind.Absolute))
                throw new Exception("Not a well format IRI string.");

            this._iriString = iriString;
        }

        public override string ToString()
        {
            return _iriString;
        }

        public static bool TryParse(string iriString, out IRI iri)
        {
            iri = null;
            try
            {
                iri = new IRI(iriString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static implicit operator IRI(string iriString)
        {
            return new IRI(iriString);
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
                IRI iri = null;
                if (IRI.TryParse(value as string, out iri))
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
