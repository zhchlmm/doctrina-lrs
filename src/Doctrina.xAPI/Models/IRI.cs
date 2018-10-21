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
            try
            {
                var url = new Uri(iriString);
            }
            catch (Exception)
            {
                throw new FormatException($"IRI '{iriString}' is not a well formatted IRI string.");
            }

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

        public override bool Equals(object obj)
        {
            var iri = obj as Iri;
            return iri != null &&
                   _iriString == iri._iriString;
        }

        public override int GetHashCode()
        {
            return 314876093 + EqualityComparer<string>.Default.GetHashCode(_iriString);
        }

        public static bool operator ==(Iri iri1, Iri iri2)
        {
            return EqualityComparer<Iri>.Default.Equals(iri1, iri2);
        }

        public static bool operator !=(Iri iri1, Iri iri2)
        {
            return !(iri1 == iri2);
        }

        public static implicit operator Uri(Iri d)
        {
            return new Uri(d.ToString());
        }

        public static implicit operator Iri(Uri d)
        {
            return new Iri(d.ToString());
        }

        public static explicit operator Iri(string strIri){
            return new Iri(strIri);
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
