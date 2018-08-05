using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace Doctrina.xAPI.Json.Schema.Providers
{
    public class OpenIDSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var openidIdentitySchema = new JSchema()
            {
                Id = new Uri("OpenIdIdentify", UriKind.Relative),
                Type = JSchemaType.Object,
                Required = { "openid" },
                Properties =
                {
                    {
                        "openid", new JSchema()
                        {
                            Id = new Uri("#openid!core", UriKind.Relative),
                            Type = JSchemaType.Object,
                            Format = "iri"
                        }
                    },
                    { "mbox", new JSchema(){ Type = JSchemaType.Null } },
                    { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.Null } },
                    { "account", new JSchema(){ Type = JSchemaType.Null } },
                }
            };

            return openidIdentitySchema;
        }
    }
}
