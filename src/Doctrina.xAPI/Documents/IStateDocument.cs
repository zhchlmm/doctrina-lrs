using System;
using Doctrina.xAPI.Models;

namespace Doctrina.xAPI.Documents
{
    public interface IStateDocument : IDocument
    {
        Activity Activity { get; set; }
        Agent Agent { get; set; }
        Guid? Registration { get; set; }
    }
}