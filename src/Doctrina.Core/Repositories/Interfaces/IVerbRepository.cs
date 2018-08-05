using System;
using Doctrina.Core.Data;

namespace Doctrina.Core.Repositories
{
    public interface IVerbRepository
    {
        void CreateVerb(VerbEntity verb);
        bool Exist(string verbId);
        VerbEntity GetByVerbId(string verbId);
        VerbEntity GetByVerbId(Uri verbId);
    }
}