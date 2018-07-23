using System;
using System.Collections.Generic;

namespace Doctrina.xAPI.Models
{
    public interface IStatementsResult
    {
        Uri More { get; set; }
        IEnumerable<Statement> Statements { get; set; }
    }
}