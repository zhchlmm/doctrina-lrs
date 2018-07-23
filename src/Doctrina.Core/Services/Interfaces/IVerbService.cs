using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.Core.Persistence.Models;
using xAPI.Core.Models;

namespace Doctrina.Core.Services
{
    public interface IVerbService
    {
        VerbEntity MergeVerb(Verb verb);
    }
}
