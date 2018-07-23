using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Core.Models
{
    public class PagedResult<T>
    {
        public PagedResult(int totalItems, int pageNumber, int limit)
        {
            this.TotalItems = totalItems;
            this.PageNumber = pageNumber;
            this.TotalPages = (int)Math.Ceiling((double)totalItems / limit);
        }

        public List<T> Items { get; set; }
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalItems { get; private set; }
    }
}
