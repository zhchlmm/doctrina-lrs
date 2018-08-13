using System;

namespace Doctrina.xAPI.Models
{
    public interface IVerb
    {
        LanguageMap Display { get; set; }
        Iri Id { get; set; }
    }
}