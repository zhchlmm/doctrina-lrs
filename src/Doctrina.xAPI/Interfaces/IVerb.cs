using System;

namespace Doctrina.xAPI
{
    public interface IVerb
    {
        LanguageMap Display { get; set; }
        Iri Id { get; set; }
    }
}