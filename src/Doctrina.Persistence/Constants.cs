using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Persistence
{
    public class Constants
    {
        public const int OBJECT_TYPE_LENGTH = 12;

        private const int MAX_PATH_LENGTH = 2048;
        private const int MAX_SCHEME_LENGTH = 32;
        public const int MAX_URL_LENGTH = MAX_PATH_LENGTH + MAX_SCHEME_LENGTH + 3;

        public static int HASH_LENGTH = 32;
    }
}
