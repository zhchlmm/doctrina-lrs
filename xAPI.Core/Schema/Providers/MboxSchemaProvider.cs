using xAPI.Core.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xAPI.Core.Schema.Providers
{
    public class MboxSchemaProvider : JSchemaGenerationProvider
    {
        public override bool CanGenerateSchema(JSchemaTypeGenerationContext context)
        {
            if(context.ObjectType == typeof(Mbox))
            {
                return true;
            }
            return false;
        }

        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            JSchema mboxSchema = new JSchema()
            {
                Id = new Uri("#mbox", UriKind.Relative),
                Type = JSchemaType.Object,
                Required = { "mbox" },
                Properties =
                {
                    {
                        "mbox", new JSchema(){
                            Id = new Uri("#mbox!core", UriKind.Relative),
                            Type = JSchemaType.String,
                            Format = "mailto-iri"
                        }
                    },
                    { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.Null } },
                    { "openid", new JSchema(){ Type = JSchemaType.Null } },
                    { "account", new JSchema(){ Type = JSchemaType.Null } },
                }
            };

            return mboxSchema;
        }
    }
}
