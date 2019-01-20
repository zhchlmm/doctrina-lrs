using Doctrina.xAPI;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Doctrina.Web.Mvc.ModelBinders
{
    /// <summary>
    /// Binds a single statement
    /// </summary>
    public class StatementPutModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                if (bindingContext == null)
                {
                    throw new ArgumentNullException(nameof(bindingContext));
                }

                // Specify a default argument name if none is set by ModelBinderAttribute
                var modelName = bindingContext.BinderModelName;
                if (string.IsNullOrEmpty(modelName))
                {
                    modelName = "statements";
                }

                //if (bindingContext.ModelType != typeof(Statement))
                //    return Task.CompletedTask;

                var request = bindingContext.ActionContext.HttpContext.Request;

                Statement statement = null;

                var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
                if(contentType.MediaType == MediaTypes.Application.Json)
                {
                    string json = null;
                    using (var streamReader = new System.Net.Http.StreamContent(request.Body))
                    {
                        json = streamReader.ReadAsStringAsync().Result;
                    }

                    statement = DeserializeStatement(bindingContext, json);
                }
                else if(contentType.MediaType == MediaTypes.Multipart.Mixed)
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
                            statement = DeserializeStatement(bindingContext, jsonString);
                        }
                        else
                        {
                            var attachmentSection = new MultipartAttachmentSection(section);
                            string hash = attachmentSection.XExperienceApiHash;
                            var attachment = statement.Attachments.SingleOrDefault(x => x.SHA2 == hash);
                            attachment.SetPayload(await attachmentSection.ReadAsByteArrayAsync());
                        }

                        section = await multipartReader.ReadNextSectionAsync();
                        sectionIndex++;
                    }
                }

                if (statement != null)
                {
                    bindingContext.Result = ModelBindingResult.Success(statement);
                }
                else
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                }

            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("", ex.Message);
            }
        }

        private Statement DeserializeStatement(ModelBindingContext bindingContext, string json)
        {
            var request = bindingContext.ActionContext.HttpContext.Request;

            JsonTextReader jsonReader = new JsonTextReader(new System.IO.StringReader(json));

            string strVersion = request.Headers[Headers.XExperienceApiVersion];
            if (string.IsNullOrWhiteSpace(strVersion))
            {
                throw new Exception($"'{Headers.XExperienceApiVersion}' header is missing.");
            }

            ApiJsonSerializer serializer = new ApiJsonSerializer(strVersion);
            serializer.Error += delegate (object sender, ErrorEventArgs args)
            {
                bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            };

            var statement = serializer.Deserialize<Statement>(jsonReader);

            return statement;
        }

    }
}
