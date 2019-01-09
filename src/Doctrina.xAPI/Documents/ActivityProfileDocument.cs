using Doctrina.xAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Documents
{
    public class ActivityProfileDocument : Document
    {
        public Iri ActivityId { get; set; }

        public string ProfileId { get; set; }

        public Guid? Registration { get; set; }
    }
}
