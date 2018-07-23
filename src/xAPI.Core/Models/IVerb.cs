using System;

namespace xAPI.Core.Models
{
    public interface IVerb
    {
        LanguageMap Display { get; set; }
        Uri Id { get; set; }
    }
}