using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Models
{
    public class InternetMediaType
    {
        private string _mediaType;

        private InternetMediaType(string mediaType)
        {
            _mediaType = mediaType;
        }

        public static implicit operator InternetMediaType(string mediaType)
        {
            return new InternetMediaType(mediaType);
        }
    }
}
