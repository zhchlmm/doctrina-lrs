using Doctrina.xAPI.Exceptions;
using Doctrina.xAPI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Doctrina.xAPI
{
    /// <summary>
    /// Internationalized Resource Identifiers, or IRIs, are unique identifiers which could also be resolvable. 
    /// IRIs can contain some characters outside of the ASCII character set.
    /// </summary>
    public class Iri
    {
        private readonly string _iriString;

        public Iri(string iriString)
        {
            try
            {
                var url = new Uri(iriString);
            }
            catch (Exception)
            {
                throw new IriFormatException($"IRI '{iriString}' is not a well formatted IRI string.");
            }

            _iriString = iriString;
        }

        public override string ToString()
        {
            return _iriString;
        }

        public static Iri Parse(string iriString)
        {
            return new Iri(iriString);
        }

        public static bool TryParse(string iriString, out Iri iri)
        {
            iri = null;
            try
            {
                iri = Parse(iriString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Compute SHA1 hash converted to hex digits
        /// </summary>
        /// <returns></returns>
        public string ComputeHash()
        {
            return SHAHelper.SHA1.ComputeHash(_iriString);
        }

        public override bool Equals(object obj)
        {
            if (obj is Iri iri)
            {
                return iri != null &&
                   _iriString == iri._iriString;
            }
            else
            {
                return false;
            }
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

        //public static bool operator ==(Iri iri, string str)
        //{
        //    return iri.ToString() == str;
        //}

        //public static bool operator !=(Iri iri, string str)
        //{
        //    return iri.ToString() != str;
        //}

        public static explicit operator Uri(Iri d)
        {
            return new Uri(d.ToString());
        }

        public static explicit operator Iri(Uri d)
        {
            return new Iri(d.ToString());
        }

        public static explicit operator Iri(string strIri)
        {
            return new Iri(strIri);
        }
    }

    public class IRITypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                if (Iri.TryParse(value as string, out Iri iri))
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
