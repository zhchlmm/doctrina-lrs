using Doctrina.Web.Models;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Doctrina.xAPI;
using Doctrina.Web.Areas.xAPI.Models;
using System.Text;

namespace Doctrina.Web.Areas.xAPI.Mvc.ModelBinders
{
    public class StatementsPostContentModelBinder : IModelBinder
    {
        private static JSchema _schema;
        private JSchema Schema
        {
            get
            {
                if (_schema == null)
                {
                    //JSchemaGenerator generator = new JSchemaGenerator();
                    //generator.SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName;
                    //generator.SchemaLocationHandling = SchemaLocationHandling.Definitions;
                    //generator.SchemaReferenceHandling = SchemaReferenceHandling.All;
                    //generator.GenerationProviders.Add(new StringEnumGenerationProvider());
                    //JSchema schema = generator.Generate(typeof(Statement));
                    ////_schema = JSchema.From.FromTypeAsync<Statement>().Result;
                    //schema.AllowAdditionalProperties = false;
                    //System.IO.File.WriteAllText("statement.schema.json", schema.ToString(), Encoding.UTF8);

                    // serialize JSchema directly to a file
                    //using (System.IO.StreamWriter file = System.IO.File.CreateText("statement.schema.json"))
                    //using (JsonTextWriter writer = new JsonTextWriter(file))
                    //{
                    //    schema.WriteTo(writer);
                    //}

                    //_schema = schema;

                    //using (System.IO.StreamReader file = System.IO.File.OpenText(@"wwwroot/result.schema.json"))
                    //using (JsonTextReader reader = new JsonTextReader(file))
                    //{
                    //    _schema = JSchema.Load(reader);

                    //    // validate JSON
                    //}

                    using (System.IO.StreamReader file = System.IO.File.OpenText(@"result.schema.json"))
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        _schema = JSchema.Load(reader);
                    }

                }
                return _schema;
            }
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(StatementsPostContent))
                return Task.CompletedTask;

            var request = bindingContext.ActionContext.HttpContext.Request;

            string json = null;
            if (request.ContentType == MIMETypes.Application.Json)
            {
                using (var streamReader = new System.Net.Http.StreamContent(request.Body))
                {
                    json = streamReader.ReadAsStringAsync().Result;
                }
            }
            else if (request.ContentType.IndexOf(MIMETypes.Multipart.Mixed) > 0)
            {
                var attachments = GetAttachments(request).Result;
            }

            JsonTextReader jsonReader = new JsonTextReader(new System.IO.StringReader(json));

            JSchemaValidatingReader validatingReader = new JSchemaValidatingReader(jsonReader);
            validatingReader.Schema = Schema;
            validatingReader.ValidationEventHandler += delegate (object sender, SchemaValidationEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.Path, args.Message);
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.CheckAdditionalContent = true;
            serializer.Error += delegate (object sender, ErrorEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            };

            if (json.StartsWith("["))
            {
                var statements = serializer.Deserialize<Statement[]>(validatingReader);
                var model = new StatementsPostContent()
                {
                    Statements = statements
                };

                bindingContext.Result = ModelBindingResult.Success(model);
            }
            else
            {
                var statement = serializer.Deserialize<Statement>(validatingReader);
                var model = new StatementsPostContent()
                {
                    Statements = new Statement[] { statement }
                };

                bindingContext.Result = ModelBindingResult.Success(model);
            }

            return Task.CompletedTask;
        }

        private async Task<List<MultipartAttachment>> GetAttachments(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var attachments = new List<MultipartAttachment>();

            var mixedContentType = MediaTypeHeaderValue.Parse(request.ContentType);
            string boundary = GetBoundary(mixedContentType, 70);
            var multipartReader = new Microsoft.AspNetCore.WebUtilities.MultipartReader(boundary, request.Body);
            var section = await multipartReader.ReadNextSectionAsync();
            while (section != null)
            {
                var contentTransferEncoding = section.Headers["Content-Transfer-Encoding"];
                if (string.IsNullOrWhiteSpace(contentTransferEncoding) || contentTransferEncoding != "binary")
                {
                    // 
                }
               
                var contentType = section.ContentType;
                if (string.IsNullOrWhiteSpace(contentType))
                {
                    // 
                }

                var xExperienceApiHash = section.Headers["X-Experience-API-Hash"];
                if (string.IsNullOrWhiteSpace(xExperienceApiHash))
                {
                    // 
                }

                var multipartAttachment = new MultipartAttachment
                {
                    SHA2 = xExperienceApiHash
                };
                using (var ms = new System.IO.MemoryStream())
                {
                    section.Body.CopyTo(ms);
                    multipartAttachment.Content = ms.ToArray();
                }

                attachments.Add(multipartAttachment);

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await multipartReader.ReadNextSectionAsync();
            }

            return attachments;
        }

        public string GetBoundary(MediaTypeHeaderValue contentType, int lengthLimit)
        {
            string boundary = null;
            var boundaryParamter = contentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
            if (boundaryParamter != null)
            {
                boundary = boundaryParamter.Value;
            }
            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new UnsupportedContentTypeException("Missing content-type boundary.");
            }

            if (boundary.Length > lengthLimit)
            {
                throw new UnsupportedContentTypeException($"Multipart boundary length limit {lengthLimit} exceeded.");
            }

            return boundary;
        }
    }
}
