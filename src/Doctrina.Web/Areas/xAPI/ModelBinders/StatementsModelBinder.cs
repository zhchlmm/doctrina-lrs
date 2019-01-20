using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.xAPI;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.Web.Areas.xAPI.Mvc.ModelBinders
{
    public class StatementsPostContentModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                if (bindingContext.ModelType != typeof(StatementsPostContent))
                    return;

                var model = new StatementsPostContent();

                var request = bindingContext.ActionContext.HttpContext.Request;

                var version = request.Headers[Headers.XExperienceApiVersion];

                var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
                if(contentType.MediaType == MediaTypes.Application.Json)
                {
                    using(System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        request.Body.CopyTo(ms);
                        string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                        model.Statements = ReadStatements(bindingContext, jsonString);
                    }
                }else if(contentType.MediaType == MediaTypes.Multipart.Mixed)
                {
                    var boundary = contentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
                    var multipartReader = new MultipartReader(boundary.Value, request.Body);
                    var section = await multipartReader.ReadNextSectionAsync();
                    int sectionIndex = 0;
                    while (section != null)
                    {
                        if (sectionIndex == 0)
                        {
                            string jsonString = await section.ReadAsStringAsync();
                            model.Statements = ReadStatements(bindingContext, jsonString);
                        }
                        else
                        {
                            var attachmentSection = new MultipartAttachmentSection(section);
                            string hash = attachmentSection.XExperienceApiHash;
                            Attachment attachment = model.Statements.Select(s => s.Attachments.SingleOrDefault(x => x.SHA2 == hash))
                                .SingleOrDefault();
                            attachment.SetPayload(await attachmentSection.ReadAsByteArrayAsync());
                        }

                        section = await multipartReader.ReadNextSectionAsync();
                        sectionIndex++;
                    }
                }

                if(model.Statements == null)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return;
                }

                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("", ex, bindingContext.ModelMetadata);
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }

        private Statement[] ReadStatements(ModelBindingContext bindingContext, string jsonString)
        {
            if (jsonString.StartsWith("{"))
            {
                var stmt = DeserializeStatement<Statement>(bindingContext, jsonString);
                return new Statement[] { stmt };
            }
            else if (jsonString.StartsWith("["))
            {
                return DeserializeStatement<Statement[]>(bindingContext, jsonString);
            }

            return null;
        }

        private T DeserializeStatement<T>(ModelBindingContext bindingContext, string json)
        {
            var request = bindingContext.ActionContext.HttpContext.Request;

            JsonTextReader jsonReader = new JsonTextReader(new System.IO.StringReader(json));

            string strVersion = request.Headers[Headers.XExperienceApiVersion];
            if (string.IsNullOrWhiteSpace(strVersion))
            {
                throw new Exception($"'{Headers.XExperienceApiVersion}' header is missing.");
            }

            ApiJsonSerializer serializer = new ApiJsonSerializer(strVersion);
            serializer.MissingMemberHandling = MissingMemberHandling.Error;
            serializer.Error += delegate (object sender, ErrorEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            };

            return serializer.Deserialize<T>(jsonReader); ;
        }
    }
}
