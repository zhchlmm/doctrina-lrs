using System;

namespace Doctrina.xAPI
{
    public interface IStatementsResult
    {
        Uri More { get; set; }
        StatementCollection Statements { get; set; }
    }
}