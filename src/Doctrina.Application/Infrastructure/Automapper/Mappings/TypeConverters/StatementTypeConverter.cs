using AutoMapper;
using Doctrina.Domain.Entities;
using Doctrina.xAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Application.Infrastructure.Automapper.Mappings.TypeConverters
{
    public class StatementTypeConverter : ITypeConverter<StatementEntity, Statement>
    {
        public Statement Convert(StatementEntity source, Statement destination, ResolutionContext context)
        {
            var stmt = new Statement(source.FullStatement);
            return stmt;
        }
    }
}
