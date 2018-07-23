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
    public class MboxValidator : JsonValidator
    {
        public const string MAILTO_IRI_PATTERN = "^mailto:(?:%[0-9a-fA-F]{2}|[-a-zA-Z0-9\\._~\\u00A0-\\uD7FF\\uF900-\\uFDCF\\uFDF0-\\uFFEF\\uDC00\\uD800-\\uDFFD\\uD83F\\uDC00\\uD840-\\uDFFD\\uD87F\\uDC00\\uD880-\\uDFFD\\uD8BF\\uDC00\\uD8C0-\\uDFFD\\uD8FF\\uDC00\\uD900-\\uDFFD\\uD93F\\uDC00\\uD940-\\uDFFD\\uD97F\\uDC00\\uD980-\\uDFFD\\uD9BF\\uDC00\\uD9C0-\\uDFFD\\uD9FF\\uDC00\\uDA00-\\uDFFD\\uDA3F\\uDC00\\uDA40-\\uDFFD\\uDA7F\\uDC00\\uDA80-\\uDFFD\\uDABF\\uDC00\\uDAC0-\\uDFFD\\uDAFF\\uDC00\\uDB00-\\uDFFD\\uDB3F\\uDC00\\uDB44-\\uDFFD\\uDB7F!\\$&'\\(\\)\\*\\+,;=:])+@(?:\\[(?:(?:(?:(?:[0-9A-Fa-f]{1,4}:){6}|::(?:[0-9A-Fa-f]{1,4}:){5}|(?:[0-9A-Fa-f]{1,4})?::(?:[0-9A-Fa-f]{1,4}:){4}|(?:(?:[0-9A-Fa-f]{1,4}:){0,1}[0-9A-Fa-f]{1,4})?::(?:[0-9A-Fa-f]{1,4}:){3}|(?:(?:[0-9A-Fa-f]{1,4}:){0,2}[0-9A-Fa-f]{1,4})?::(?:[0-9A-Fa-f]{1,4}:){2}|(?:(?:[0-9A-Fa-f]{1,4}:){0,3}[0-9A-Fa-f]{1,4})?::[0-9A-Fa-f]{1,4}:|(?:(?:[0-9A-Fa-f]{1,4}:){0,4}[0-9A-Fa-f]{1,4})?::)(?:[0-9A-Fa-f]{1,4}:[0-9A-Fa-f]{1,4}|(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))|(?:(?:[0-9A-Fa-f]{1,4}:){0,5}[0-9A-Fa-f]{1,4})?::[0-9A-Fa-f]{1,4}|(?:(?:[0-9A-Fa-f]{1,4}:){0,6}[0-9A-Fa-f]{1,4})?::)|[Vv][0-9A-Fa-f]+\\.[A-Za-z0-9\\-._~!$&'()*+,;=:]+)\\]|(?:(?:25[0-5]|2[0-4] [0-9]|[01]?[0 - 9][0 - 9]?)\\.){3}";
        public const string MAILTO_URI_PATTERN = "^mailto:(?:(?:(?:[A-Za-z0-9\\-._~!$&'()*+,;=:]|%[0-9A-Fa-f]{2})+)@(?:(?:\\[(?:(?:(?:(?:[0-9A-Fa-f]{1,4}:){6}|::(?:[0-9A-Fa-f]{1,4}:){5}|(?:[0-9A-Fa-f]{1,4})?::(?:[0-9A-Fa-f]{1,4}:){4}|(?:(?:[0-9A-Fa-f]{1,4}:){0,1}[0-9A-Fa-f]{1,4})?::(?:[0-9A-Fa-f]{1,4}:){3}|(?:(?:[0-9A-Fa-f]{1,4}:){0,2}[0-9A-Fa-f]{1,4})?::(?:[0-9A-Fa-f]{1,4}:){2}|(?:(?:[0-9A-Fa-f]{1,4}:){0,3}[0-9A-Fa-f]{1,4})?::[0-9A-Fa-f]{1,4}:|(?:(?:[0-9A-Fa-f]{1,4}:){0,4}[0-9A-Fa-f]{1,4})?::)(?:[0-9A-Fa-f]{1,4}:[0-9A-Fa-f]{1,4}|(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))|(?:(?:[0-9A-Fa-f]{1,4}:){0,5}[0-9A-Fa-f]{1,4})?::[0-9A-Fa-f]{1,4}|(?:(?:[0-9A-Fa-f]{1,4}:){0,6}[0-9A-Fa-f]{1,4})?::)|[Vv][0-9A-Fa-f]+\\.[A-Za-z0-9\\-._~!$&'()*+,;=:]+)\\]|(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)|(?:[A-Za-z0-9\\-._~!$&'()*+,;=]|%[0-9A-Fa-f]{2})+)))$";

        public override bool CanValidate(JSchema schema)
        {
            if (schema.Format == "mbox-uri" || schema.Format == "mbox-iri")
            {
                return true;
            }
            return false;
        }

        public override void Validate(JToken value, JsonValidatorContext context)
        {
            if(value.Type == JTokenType.String)
            {
                string s = value.ToString();
                if (context.Schema.Format == "mbox-uri")
                {
                    if (!Regex.Match(value.Value<string>(), MAILTO_URI_PATTERN).Success)
                    {
                        context.RaiseError($"Text '{s}' is not a valid mbox URI.");
                    }
                }

                if (context.Schema.Format == "mbox-iri")
                {
                    if (!Regex.Match(value.Value<string>(), MAILTO_IRI_PATTERN).Success)
                    {
                        context.RaiseError($"Text '{s}' is not a valid mbox IRI.");
                    }
                }
            }
        }
    }
}
