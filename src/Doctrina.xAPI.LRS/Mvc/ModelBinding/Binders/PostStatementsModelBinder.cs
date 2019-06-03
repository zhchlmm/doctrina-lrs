using Doctrina.xAPI.Collections;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.LRS.Http;
using Doctrina.xAPI.LRS.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Doctrina.xAPI.LRS.Mvc.ModelBinding
{
    public class PostStatementsModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(PostStatementContent))
            {
                return;
            }

            var model = new PostStatementContent();

            var request = bindingContext.ActionContext.HttpContext.Request;

            var contentType = MediaTypeHeaderValue.Parse(request.ContentType);

            var jsonModelReader = new JsonModelReader(contentType, request.Body);
            model.Statements = await jsonModelReader.ReadAs<StatementCollection>();

            // TODO: Throw exception with parsing errors
            JsonModelErrorsCollection parsingErrors = model.Statements.GetErrorsOfDescendantsAndSelf();
            if (parsingErrors.Count > 0)
            {
                foreach (var item in parsingErrors)
                {
                    bindingContext.ModelState.AddModelError(item.Name, item.ErrorMessage);
                }
            }

            if(model.Statements == null)
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
    }
}
