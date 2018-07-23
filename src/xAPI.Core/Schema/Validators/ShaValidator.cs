using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xAPI.Core.Schema.Validators
{
    public class ShaValidator : JsonValidator
    {
        public const string SHA1_PATTERN = @"^[0-9a-fA-F]{40}$";
        public const string SHA2_PATTERN = @"^[0-9a-fA-F]{56}(?:[0-9a-fA-F]{8}(?:[0-9a-fA-F]{32}){0,2})?$";

        public override bool CanValidate(JSchema schema)
        {
            if(schema.Format == "sha1" || schema.Format == "sha2")
            {
                return true;
            }
            return false;
        }

        public override void Validate(JToken value, JsonValidatorContext context)
        {
            if(context.Schema.Format == "sha1")
            {
                if (!Regex.Match(value.Value<string>(), SHA1_PATTERN).Success)
                {
                    throw new JSchemaValidationException("Not a valid SHA1 format.");

                }
            }

            if (context.Schema.Format == "sha2")
            {
                if (!Regex.Match(value.Value<string>(), SHA2_PATTERN).Success)
                {
                    throw new JSchemaValidationException("Not a valid SHA2 format.");
                }
            }
        }
    }
}
