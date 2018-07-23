using Doctrina.xAPI.Models;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.Schema.Providers
{
    public class ContextActivitiesSchemaProvider : JSchemaGenerationProvider
    {
        public override JSchema GetSchema(JSchemaTypeGenerationContext context)
        {
            var activitySchema = context.Generator.Generate(typeof(Activity));

            //string schemaJson = @"{
            //    'type': 'object',
            //    'properties': {
            //        'parent': {
            //            'oneOf':[
            //                {
            //                    'type':'array',
            //                    'itens':{'$ref': '#/definitions/activity'}
            //                },
            //                {'$ref': '#/definitions/activity'}
            //            ]
            //        },
            //        'grouping': {
            //            'oneOf':[
            //                {
            //                    'type':'array',
            //                    'itens':{'$ref': '#/definitions/activity'}
            //                },
            //                {'$ref': '#/definitions/activity'}
            //            ]
            //        },
            //        'category': {
            //            'oneOf':[
            //                {
            //                    'type':'array',
            //                    'itens':{'$ref': '#/definitions/activity'}
            //                },
            //                {'$ref': '#/definitions/activity'}
            //            ]
            //        },
            //        'other': {
            //            'oneOf':[
            //                {
            //                    'type':'array',
            //                    'itens':{'$ref': '#/definitions/activity'}
            //                },
            //                {'$ref': '#/definitions/activity'}
            //            ]
            //        }
            //    },
            //    'definitions':{ 
            //        'activity':" + activitySchema.ToString() + @"
            //    }
            //}";

            var oneOf = new JSchema()
            {
                OneOf =
                {
                    new JSchema()
                    {
                        Type = JSchemaType.Array,
                        Items = { activitySchema }
                    },
                    activitySchema,
                }
            };
            //}(@"{
            //    'oneOf':[
            //        {
            //            'type':'array',
            //            'itens':{'$ref': '#/definitions/activity'}
            //        },
            //        {'$ref': '#/definitions/activity'}
            //    ]
            //}");


            var caSchema = new JSchema()
            {
                Description = "A person",
                Type = JSchemaType.Object,
                AllowAdditionalProperties = false,
                Properties =
                {
                    { "parent", oneOf },
                    { "grouping", oneOf },
                    { "category", oneOf },
                    { "other", oneOf }
                }
            };
            //var schema = JSchema.Parse(schemaJson);
            return caSchema;
        }
    }
}
