using System;
using System.Collections.Generic;

namespace Doctrina.xAPI
{
    public interface IStatementsResult
    {
        Uri More { get; set; }
        StatementCollection Statements { get; set; }
    }
}