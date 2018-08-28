using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
    public struct Duration
    {
        private static char[] _dateDesignators = new char[] { 'P', 'Y', 'M', 'W', 'D' };
        private static char[] _timeDesignators = new char[] { 'P', 'T', 'H', 'M', 'S' };

        public long Ticks;

        public double? Years;
        public double? Months;
        public double? Weeks;
        public double? Days;

        public double? Hours;
        public double? Minutes;
        public double? Seconds;

        public Duration(string s)
        {
            string strDuration = s.Trim();

            if (!strDuration.StartsWith("P"))
                throw new Exception("Duration must start with the designator 'P' (for period).");

            if (strDuration.Length == 1)
                throw new Exception("'P' designator is not valid for duration alone.");

            Years = null;
            Months = null;
            Weeks = null;
            Days = null;
            Hours = null;
            Minutes = null;
            Seconds = null;
            Ticks = 0;

            List<KeyValuePair<char, double>> elements = GetElements(strDuration);
            AddElements(ref this, elements);
        }

        public Duration(long ticks)
        {
            Weeks = null;
            Ticks = ticks;
            var t = TimeSpan.FromTicks(ticks);
            var totalDays = t.TotalDays;
            Years = Math.Truncate(totalDays / 365);
            Months = Math.Truncate((totalDays % 365) / 30);
            Days = Math.Truncate(((totalDays % 365) % 7) % 30);
            Hours = t.Hours;
            Minutes = t.Minutes;
            Seconds = t.Seconds;
        }

        public static Duration Parse(string s)
        {
            return new Duration(s);
        }

        private static List<KeyValuePair<char, double>> GetElements(string s)
        {
            string strValue = "";
            string strDuration = s.Trim();
            char pre;
            var elements = new List<KeyValuePair<char, double>>();
            for (int i = 0; i < strDuration.Length; i++)
            {
                var chr = strDuration[i];

                // We expect digits first
                if (char.IsDigit(chr) || chr == '.')
                {
                    strValue += chr;
                    continue;
                }
                else if (char.IsLetter(chr))
                {
                    // Skip if period or time designator
                    if (chr == 'T' || chr == 'P')
                    {
                        pre = chr;
                        elements.Add(new KeyValuePair<char, double>(chr, 0.0));
                        continue;
                    }

                    var designator = chr;
                    if (!_dateDesignators.Contains(chr) && !_timeDesignators.Contains(chr))
                    {
                        throw new Exception($"'{chr}' is not a valid designator.");
                    }

                    // We have received digits
                    if (string.IsNullOrEmpty(strValue))
                    {
                        throw new Exception($"'{chr}' designator must have a value.");
                    }

                    double value;
                    if (double.TryParse(strValue, out value))
                    {
                        elements.Add(new KeyValuePair<char, double>(chr, value));
                        strValue = null; // Clear value
                    }
                    else
                    {
                        throw new Exception($"'{strValue}' is not valid for designator '{chr}'.");
                    }
                }
            }

            return elements;
        }

        private static void AddElements(ref Duration d, List<KeyValuePair<char, double>> elements)
        {
            char pt = 'P';
            foreach (var element in elements)
            {
                if (element.Key == 'P')
                    continue;

                if (element.Key == 'T')
                {
                    pt = 'T';
                    continue;
                }

                if (pt == 'P')
                {
                    AddPeriod(ref d, element);
                }
                else if (pt == 'T')
                {
                    AddTime(ref d, element);
                }
            }
        }

        private static void AddPeriod(ref Duration d, KeyValuePair<char, double> element)
        {
            switch (element.Key)
            {
                case 'Y':
                    d.Years = element.Value;
                    d.Ticks += (long)Math.Floor((TimeSpan.TicksPerDay * 365.242199) * element.Value);
                    break;
                case 'M':
                    d.Months = element.Value;
                    d.Ticks += (long)Math.Floor((TimeSpan.TicksPerDay * 30.4368499) * element.Value);
                    break;
                case 'W':
                    d.Weeks = element.Value;
                    d.Ticks += (long)Math.Floor((TimeSpan.TicksPerDay * 7) * element.Value);
                    break;
                case 'D':
                    d.Days = element.Value;
                    d.Ticks += (long)Math.Floor(TimeSpan.TicksPerDay * element.Value);
                    break;
            }
        }

        private static void AddTime(ref Duration d, KeyValuePair<char, double> element)
        {
            switch (element.Key)
            {
                case 'H':
                    d.Hours = element.Value;
                    d.Ticks += (long)Math.Floor(TimeSpan.TicksPerHour * element.Value);
                    break;
                case 'M':
                    d.Minutes = element.Value;
                    d.Ticks += (long)Math.Floor(TimeSpan.TicksPerMinute * element.Value);
                    break;
                case 'S':
                    d.Seconds = element.Value;
                    d.Ticks += (long)Math.Floor(TimeSpan.TicksPerSecond * element.Value);
                    break;
            }
        }

        public static Duration FromTicks(long ticks)
        {
            return new Duration(ticks);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("P");
            if (Years.HasValue)
            {
                sb.AppendFormat("{0}Y", Years.Value);
            }
            if (Months.HasValue)
            {
                sb.AppendFormat("{0}Y", Months.Value);
            }
            if (Weeks.HasValue)
            {
                sb.AppendFormat("{0}Y", Weeks.Value);
            }
            if (Days.HasValue)
            {
                sb.AppendFormat("{0}Y", Days.Value);
            }

            if (Hours.HasValue || Minutes.HasValue || Seconds.HasValue)
            {
                sb.Append("T");
                if (Hours.HasValue)
                {
                    sb.AppendFormat("{0}H", Hours.Value);
                }
                if (Minutes.HasValue)
                {
                    sb.AppendFormat("{0}M", Minutes.Value);
                }
                if (Seconds.HasValue)
                {
                    sb.AppendFormat("{0}S", Seconds.Value);
                }
            }

            return sb.ToString();
        }

        public static bool TryParse(string durationString, out Duration duration)
        {
            duration = new Duration();
            try
            {
                duration = new Duration(durationString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Duration))
            {
                return false;
            }

            var duration = (Duration)obj;
            return Ticks == duration.Ticks &&
                   EqualityComparer<double?>.Default.Equals(Years, duration.Years) &&
                   EqualityComparer<double?>.Default.Equals(Months, duration.Months) &&
                   EqualityComparer<double?>.Default.Equals(Weeks, duration.Weeks) &&
                   EqualityComparer<double?>.Default.Equals(Days, duration.Days) &&
                   EqualityComparer<double?>.Default.Equals(Hours, duration.Hours) &&
                   EqualityComparer<double?>.Default.Equals(Minutes, duration.Minutes) &&
                   EqualityComparer<double?>.Default.Equals(Seconds, duration.Seconds);
        }

        public override int GetHashCode()
        {
            var hashCode = 1706700940;
            hashCode = hashCode * -1521134295 + Ticks.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Years);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Months);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Weeks);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Days);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Hours);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Minutes);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Seconds);
            return hashCode;
        }

        public static bool operator ==(Duration left, Duration right)
        {
            return left.ToString() == right.ToString();
        }

        public static bool operator !=(Duration left, Duration right)
        {
            return !(left == right);
        }

        public static bool operator <(Duration left, Duration right)
        {
            return left.Ticks < right.Ticks;
        }

        public static bool operator >(Duration left, Duration right)
        {
            return left.Ticks > right.Ticks;
        }
    }

    internal class DurationTypeConverter : TypeConverter
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
                var duration = new Duration();
                if (Duration.TryParse(value as string, out duration))
                {
                    return duration;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Duration))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }
    }
}
