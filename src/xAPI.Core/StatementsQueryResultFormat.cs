using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core
{
    public sealed class StatementsQueryResultFormat
    {
        public static readonly StatementsQueryResultFormat IDS = new StatementsQueryResultFormat("ids");
        public static readonly StatementsQueryResultFormat EXACT = new StatementsQueryResultFormat("exact");
        public static readonly StatementsQueryResultFormat CANONICAL = new StatementsQueryResultFormat("canonical");

        private readonly string format;

        private StatementsQueryResultFormat(string value)
        {
            format = value;
        }

        public static implicit operator StatementsQueryResultFormat(string format)
        {
            return new StatementsQueryResultFormat(format);
        }

        public override String ToString()
        {
            return format;
        }
    }
}
