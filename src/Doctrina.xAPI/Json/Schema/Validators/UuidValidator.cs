using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Json.Schema.Validators
{
    public class UuidValidator : JsonValidator
    {
        public override bool CanValidate(JSchema schema)
        {
            throw new NotImplementedException();
        }

        public override void Validate(JToken value, JsonValidatorContext context)
        {
            throw new NotImplementedException();
        }
    }
}
