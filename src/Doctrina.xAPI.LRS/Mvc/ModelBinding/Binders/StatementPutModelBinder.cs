using Doctrina.xAPI;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Doctrina.WebUI.Mvc.ModelBinders
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

                string strVersion = request.Headers[Headers.XExperienceApiVersion];
                if (string.IsNullOrWhiteSpace(strVersion))
                {
                    throw new Exception($"'{Headers.XExperienceApiVersion}' header is missing.");
                }

                Statement statement = null;

                var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
                if (contentType.MediaType == MediaTypes.Application.Json)
                {
                    statement = DeserializeStatement(bindingContext, request.Body, strVersion);
                }
                else if (contentType.MediaType == MediaTypes.Multipart.Mixed)
                {
                    var boundary = contentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
                    var multipartReader = new MultipartReader(boundary.Value, request.Body);
                    var section = await multipartReader.ReadNextSectionAsync();
                    int sectionIndex = 0;
                    while (section != null)
                    {
                        if (sectionIndex == 0)
                        {
                            statement = DeserializeStatement(bindingContext, section.Body, strVersion);
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
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }

        private Statement DeserializeStatement(ModelBindingContext bindingContext, Stream jsonStream, ApiVersion version)
        {
            var serializer = new ApiJsonSerializer(version);

            using (StreamReader streamReader = new StreamReader(jsonStream, System.Text.Encoding.UTF8))
            {
                using (JsonReader jsonReader = new JsonTextReader(streamReader))
                {
                    if (!jsonReader.Read())
                    {
                        // Empty
                        return null;
                    }

                    if (jsonReader.TokenType != JsonToken.StartObject)
                    {
                        throw new JsonSerializationException("Expected JSON start with Start Object or Start Array.");
                    }

                    serializer.Error += delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    };

                    return serializer.Deserialize<Statement>(jsonReader);
                }
            }
        }

        //private void AddValidationError(ModelBindingContext bindingContext, ValidationError error)
        //{
        //    if (error.Value != null)
        //    {
        //        bindingContext.ModelState.AddModelError(error.Path, error.Message);
        //        return;
        //    }

        //    foreach (var childError in error.ChildErrors)
        //    {
        //        AddValidationError(bindingContext, childError);
        //    }
        //}
    }
}
