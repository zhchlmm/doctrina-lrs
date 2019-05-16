using Doctrina.xAPI.Http;
using Doctrina.xAPI.Json;
using Doctrina.xAPI.LRS.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.LRS.Mvc.ModelBinding
{
    public class StatementsPostModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                if (bindingContext.ModelType != typeof(StatementsPostContent))
                    return;

                var model = new StatementsPostContent();

                var request = bindingContext.ActionContext.HttpContext.Request;

                string strVersion = request.Headers[Headers.XExperienceApiVersion];
                if (string.IsNullOrWhiteSpace(strVersion))
                {
                    throw new Exception($"'{Headers.XExperienceApiVersion}' header is missing.");
                }

                var contentType = MediaTypeHeaderValue.Parse(request.ContentType);
                if (contentType.MediaType == MediaTypes.Application.Json)
                {
                    model.Statements = DeserializeStatement(bindingContext, request.Body, strVersion);
                }
                else if (contentType.MediaType == MediaTypes.Multipart.Mixed)
                {
                    var boundary = contentType.Parameters.FirstOrDefault(x => x.Name == "boundary");
                    if (boundary == null || string.IsNullOrWhiteSpace(boundary.Value))
                        throw new Exception("Content-Type parameter boundary is null or empty.");

                    var multipartReader = new MultipartReader(boundary.Value, request.Body);
                    var section = await multipartReader.ReadNextSectionAsync();
                    int sectionIndex = 0;
                    AttachmentCollection attachments = new AttachmentCollection();
                    while (section != null)
                    {
                        var sectionContentType = MediaTypeHeaderValue.Parse(section.ContentType);
                        if (sectionIndex == 0)
                        {
                            if (sectionContentType.MediaType != MediaTypes.Application.Json)
                                throw new Exception("First document part must have a Content-Type header value of \"application/json\"");

                            model.Statements = DeserializeStatement(bindingContext, section.Body, strVersion);
                            foreach (var stmt in model.Statements)
                            {
                                foreach (var attachment in stmt.Attachments)
                                {

                                }
                            }
                        }
                        else
                        {
                            var attachmentSection = new MultipartAttachmentSection(section);
                            string hash = attachmentSection.XExperienceApiHash;
                            Attachment attachment = model.Statements
                                .Select(s => s.Attachments.SingleOrDefault(x => x.SHA2 == hash))
                                .SingleOrDefault();

                            if (attachment == null)
                            {
                                throw new Exception($"No attachment match found for '{hash}'");
                            }
                            attachment.SetPayload(await attachmentSection.ReadAsByteArrayAsync());
                        }

                        section = await multipartReader.ReadNextSectionAsync();
                        sectionIndex++;
                    }
                }
                else
                {
                    throw new UnsupportedContentTypeException("Content-Type header must be application/json or multipart/mixed.");
                }

                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.AddModelError("Request body", ex.Message);
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }

        private Statement[] DeserializeStatement(ModelBindingContext bindingContext, Stream jsonStream, ApiVersion version)
        {
            var serializer = new ApiJsonSerializer(version);

            using (StreamReader streamReader = new StreamReader(jsonStream, Encoding.UTF8))
            {
                using (JsonReader jsonReader = new JsonTextReader(streamReader))
                {
                    if (!jsonReader.Read())
                    {
                        // Empty
                        return null;
                    }

                    if (!(jsonReader.TokenType == JsonToken.StartArray || jsonReader.TokenType == JsonToken.StartObject))
                    {
                        throw new JsonSerializationException("Expected JSON start with Start Object or Start Array.");
                    }
                    bool isArray = jsonReader.TokenType == JsonToken.StartArray;

                    serializer.Error += delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
                    {
                        bindingContext.ModelState.AddModelError(args.ErrorContext.Path, args.ErrorContext.Error.Message);
                        args.ErrorContext.Handled = true;
                    };

                    if (isArray)
                    {
                        return serializer.Deserialize<Statement[]>(jsonReader);
                    }
                    else
                    {
                        var statement = serializer.Deserialize<Statement>(jsonReader);
                        if (statement == null)
                            return null;

                        return new Statement[] { statement };
                    }
                }
            }
        }

        private void AddValidationError(ModelBindingContext bindingContext, ValidationError error)
        {
            if (error.Value != null)
            {
                bindingContext.ModelState.AddModelError(error.Path, error.Message);
                return;
            }

            foreach (var childError in error.ChildErrors)
            {
                AddValidationError(bindingContext, childError);
            }
        }
    }
}
