using System;

namespace Doctrina.xAPI.Models
{
    public interface IVerb
    {
        LanguageMap Display { get; set; }
        Uri Id { get; set; }
    }
}