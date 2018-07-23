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
    public class AgentSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            //JSchema mboxSchema = new JSchema()
            //{
            //    Id = new Uri("#mbox", UriKind.Relative),
            //    Type = JSchemaType.Object,
            //    Required = { "mbox" },
            //    Properties =
            //    {
            //        {
            //            "mbox", new JSchema(){
            //                Id = new Uri("#mbox!core", UriKind.Relative),
            //                Type = JSchemaType.String,
            //                Format = "mailto-iri"
            //            }
            //        },
            //        { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.Null } },
            //        { "openid", new JSchema(){ Type = JSchemaType.Null } },
            //        { "account", new JSchema(){ Type = JSchemaType.Null } },
            //    }
            //};
            JSchema mboxSchema = context.Generator.Generate(typeof(Mbox));


            var mbox_sha1sumSchema = new JSchema()
            {
                Id = new Uri("MboxSHA1SUM", UriKind.Relative),
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

            var openidSchema = new JSchema()
            {
                Id = new Uri("OpenId", UriKind.Relative),
                Type = JSchemaType.Object,
                Required = { "openid" },
                Properties =
                {
                    {
                        "openid", new JSchema()
                        {
                            Id= new Uri("#openid!core", UriKind.Relative),
                            Type = JSchemaType.Object,
                            Format = "rfc3986-uri"
                        }
                    },
                    { "mbox", new JSchema(){ Type = JSchemaType.Null } },
                    { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.Null } },
                    { "account", new JSchema(){ Type = JSchemaType.Null } },
                }
            };

            var accountSchema = new JSchema()
            {
                Id = new Uri("Account", UriKind.Relative),
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
                                { "home", new JSchema(){ Type = JSchemaType.String} }
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
                    mboxSchema,
                    mbox_sha1sumSchema,
                    openidSchema,
                    accountSchema
                },
                //ExtensionData =
                //{
                //    ["definitions"] = new JObject()
                //    {
                //        ["mbox"] = mboxSchema,
                //        ["mbox_sha1sum"] = mbox_sha1sumSchema,
                //        ["openid"] = openidSchema,
                //        ["account"] = accountSchema
                //    }
                //}
            };

            var agentSchema = new JSchema()
            {
                Title = "Agent",
                Id = new Uri("Agent", UriKind.Relative),
                Type = JSchemaType.Object,
                
                AllOf = { inverseFunctionalSchema },
                Properties =
                {
                    { "name", new JSchema(){ Type = JSchemaType.String } },
                    { "objectType", new JSchema(){ Enum = { "Agent" } } },
                    { "mbox", new JSchema(){ Type = JSchemaType.String } },
                    { "mbox_sha1sum", new JSchema(){ Type = JSchemaType.String } },
                    { "account", new JSchema(){ Type = JSchemaType.Object } },
                    { "openid", new JSchema(){ Type = JSchemaType.String } }
                }//,
                //ExtensionData = {
                //    ["definitions"] = new JObject()
                //    {
                //        ["inversefunctional"] = inverseFunctionalSchema,
                //    }
                //}
            };

            return agentSchema;
        }
    }
}
