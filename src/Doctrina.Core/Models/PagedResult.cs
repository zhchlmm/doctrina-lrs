using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Models
{
    public class PagedResult<TItem>
    {
        public PagedResult(long id, DateTime timestamp)
        {
            this.Id = id;
            this.Timestamp = timestamp;
        }

        public List<TItem> Items { get; set; }
        public long Id { get; }
        public DateTime Timestamp { get; }

        public override string ToString()
        {
            return $"{Id}_{Timestamp}";
        }
    }

    public class ContinuationToken
    {
        /// <summary>
        /// The Timestamp_ID Continuation Token
        /// </summary>
        /// <param name="timestamp">The timestamp of the last element of the current page.</param>
        /// <param name="id">The ID (primary key) of the last element of the current page. </param>
        public ContinuationToken(DateTime timestamp, int id)
        {
            Timestamp = timestamp;
            Id = id;
        }

        public ContinuationToken(string token) {
            var parts = token.Split('_');
            Timestamp = DateTime.ParseExact(parts[0], "o", System.Globalization.CultureInfo.InvariantCulture);
            Id = long.Parse(parts[1]);
        }

        /// <summary>
        /// The timestamp of the last element of the current page.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// The ID (primary key) of the last element of the current page.
        /// </summary>
        public long Id { get; }

        public override string ToString()
        {
            return $"{Timestamp.ToString("O")}_{Id}";
        }
    }
}
