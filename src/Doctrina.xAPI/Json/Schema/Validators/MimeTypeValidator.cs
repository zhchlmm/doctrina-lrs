using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Schema.Validators
{
    public class MimeTypeValidator : JsonValidator
    {
        public const string MIME_TYPE_FORMAT = "^[-\\w\\+\\.]+/[-\\w\\+\\.]+(?:;\\s*[-\\w\\+\\.]+=(?:(['\"]?)[-\\w\\+\\.]+\\1)|''|\"\")?$";

        public override bool CanValidate(JSchema schema)
        {
            if (schema.Format == "mimetype")
            {
                return true;
            }
            return false;
        }

        public override void Validate(JToken value, JsonValidatorContext context)
        {
            if (context.Schema.Format == "mimetype")
            {
                if (!Regex.Match(value.Value<string>(), MIME_TYPE_FORMAT).Success)
                {
                    throw new JSchemaValidationException("Not a valid mimetype format");
                }
            }
        }
    }
}
