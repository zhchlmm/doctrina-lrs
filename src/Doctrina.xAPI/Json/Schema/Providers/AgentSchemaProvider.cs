using Doctrina.xAPI.Json.Schema.Providers;
using Doctrina.xAPI.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Schema.Providers
{
    public class AgentSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var mboxIdentitySchema = new JSchema()
            {
                Id = new Uri("MboxIdentity", UriKind.Relative),
                Type = JSchemaType.Object,
                Required = { "mbox_sha1sum" },
                Properties =
                {
                    { "mbox", new JSchema(){ Type = JSchemaType.String, Format = "mbox" } },
                    { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.Null }},
                    { "openid", new JSchema(){ Type = JSchemaType.Null } },
                    { "account", new JSchema(){ Type = JSchemaType.Null } },
                }
            };

            var mbox_sha1sumIdentitySchema = new JSchema()
            {
                Id = new Uri("MboxSHA1SumIdentity", UriKind.Relative),
                Type = JSchemaType.Object,
                Required = { "mbox_sha1sum" },
                Properties =
                {
                    {
                        "mbox_sha1sum", new JSchema()
                        {
                            Id= new Uri("#mbox_sha1sum!core", UriKind.Relative),
                            Type = JSchemaType.Object,
                            Format = "sha1"
                        }
                    },
                    { "mbox", new JSchema(){ Type = JSchemaType.Null } },
                    { "openid", new JSchema(){ Type = JSchemaType.Null } },
                    { "account", new JSchema(){ Type = JSchemaType.Null } },
                }
            };

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
            //context.Generator.GenerationProviders.Add(openIdSchemaProvider);
            //var openidSchema = openIdSchemaProvider.GetSchema(context);

            var accountIdentitySchema = new JSchema()
            {
                Id = new Uri("AccountIdentity", UriKind.Relative),
                Type = JSchemaType.Object,
                Required = { "account" },
                Properties =
                {
                    {
                        "account", new JSchema()
                        {
                            Id =new Uri("#account!core", UriKind.Relative),
                            Type = JSchemaType.Object,
                            AllowAdditionalProperties = false,
                            Required = { "homePage", "name" },
                            Properties =
                            {
                                { "homePage", new JSchema(){ Type = JSchemaType.String, Format = "iri" } },
                                { "name", new JSchema(){ Type = JSchemaType.String} }
                            }
                        }
                    },
                    { "mbox", new JSchema(){ Type = JSchemaType.Null } },
                    { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.Null } },
                    { "openid", new JSchema(){ Type = JSchemaType.Null } },
                }
            };

            var inverseFunctionalSchema = new JSchema()
            {
                Id = new Uri("InverseFunctional", UriKind.Relative),
                OneOf =
                {
                    mboxIdentitySchema,
                    mbox_sha1sumIdentitySchema,
                    openidIdentitySchema,
                    accountIdentitySchema
                }
            };

            var agentSchema = new JSchema()
            {
                Title = "Agent",
                Id = new Uri("Agent", UriKind.Relative),
                Type = JSchemaType.Object,
                AllowAdditionalProperties = false,
                AllOf = { inverseFunctionalSchema },
                Properties =
                {
                    { "name", new JSchema(){ Type = JSchemaType.String } },
                    { "objectType", new JSchema(){ Enum = { "Agent" } } },
                    { "mbox", new JSchema(){ Type = JSchemaType.String } },
                    { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.String } },
                    { "account", new JSchema(){ Type = JSchemaType.Object } },
                    { "openid", new JSchema(){ Type = JSchemaType.String } }
                },
                ExtensionData = {
                    ["definitions"] = new JObject()
                    {
                        ["InverseFunctionalIdentifiers"] = inverseFunctionalSchema,
                        ["MboxIdentity"] = mboxIdentitySchema,
                        ["MboxSHA1SumIdentity"] = mbox_sha1sumIdentitySchema,
                        ["OpenIDIdentity"] = openidIdentitySchema,
                        ["AccountIdentity"] = accountIdentitySchema
                    }
                }
            };

            return agentSchema;
        }
    }
}
