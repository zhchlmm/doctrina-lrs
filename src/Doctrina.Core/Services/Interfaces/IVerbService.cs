using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doctrina.Core.Data;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Services
{
    public interface IVerbService
    {
        VerbEntity MergeVerb(Verb verb);
    }
}
