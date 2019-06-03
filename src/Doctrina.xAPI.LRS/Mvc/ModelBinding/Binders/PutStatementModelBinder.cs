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
    public class PutStatementModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            // Specify a default argument name if none is set by ModelBinderAttribute
            //var modelName = bindingContext.BinderModelName;
            //if (string.IsNullOrEmpty(modelName))
            //{
            //    modelName = "statements";
            //}

            var request = bindingContext.ActionContext.HttpContext.Request;

            var contentType = MediaTypeHeaderValue.Parse(request.ContentType);

            var jsonModelReader = new JsonModelReader(contentType, request.Body);
            Statement statement = await jsonModelReader.ReadAs<Statement>();

            if (statement != null)
            {
                bindingContext.Result = ModelBindingResult.Success(statement);
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }
        }
    }
}
