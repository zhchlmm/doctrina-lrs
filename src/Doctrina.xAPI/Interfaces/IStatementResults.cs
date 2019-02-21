using System;

namespace Doctrina.xAPI
{
    public interface IStatementsResult
    {
        Uri More { get; set; }
        Statement[] Statements { get; set; }
    }
}