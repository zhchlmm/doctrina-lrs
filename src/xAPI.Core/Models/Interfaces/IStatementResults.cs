using System;
using System.Collections.Generic;

namespace xAPI.Core.Models
{
    public interface IStatementsResult
    {
        Uri More { get; set; }
        IEnumerable<Statement> Statements { get; set; }
    }
}