using System;
using Doctrina.Core.Data;
using Doctrina.xAPI.Models;

namespace Doctrina.Core.Repositories
{
    public interface IVerbRepository
    {
        void CreateVerb(VerbEntity verb);
        bool Exist(string verbId);
        VerbEntity GetByVerbId(string verbId);
        VerbEntity GetByVerbId(Iri verbId);
    }
}