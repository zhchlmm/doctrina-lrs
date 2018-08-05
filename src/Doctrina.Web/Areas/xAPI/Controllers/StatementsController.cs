using Doctrina.Core.Data;
using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.Web.Mvc.ModelBinders;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Doctrina.Web.Areas.xAPI.Controllers
{
    /// <summary>
    /// The basic communication mechanism of the Experience API.
    /// </summary>
    //[LRSAuthortize]
    //[ApiVersion]
    [VersionHeader]
    [Route("xapi/statements")]
    [Produces("application/json")]
    public class StatementsController : ApiControllerBase
    {
        private readonly IStatementService _statementService;
        private readonly ILogger<StatementsController> _logger;

        public StatementsController(IStatementService statementService, ILogger<StatementsController> logger)
        {
            _statementService = statementService;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json", "multipart/mixed")]
        public IActionResult GetStatements([FromQuery]StatementsQuery parameters)
        {
            //LogHelper.Debug<StatementsController>("GetStatements \r\n{0}", () => Request.RequestUri.ToString());

            if (parameters == null)
                parameters = new StatementsQuery();

            if (parameters.StatementId.HasValue || parameters.VoidedStatementId.HasValue)
                return GetStatement(parameters.StatementId, parameters.VoidedStatementId);

            try
            {
                StatementsResult result = new StatementsResult();
                var statements = this._statementService.GetStatements(parameters);
                var attachments = new List<AttachmentEntity>();
                bool more = true;
                // Generate continueToken
                if (more)
                {
                    parameters.Since = statements.Last().Stored;
                    // TODO: Implement continue token
                    //string path = Request.Path;

                    //var continueToken = new ContinueToken();

                    //string token = continueToken.ToString();
                    //result.More = new Uri($"{path}?con{query}", UriKind.Relative);
                    //Url.Action("GetMoreStatement", new { cursor= ]})

                    result.More = new Uri(Url.Action("GetStatements", parameters));
                }

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

                bool includeAttachements = parameters.Attachments.GetValueOrDefault();
                if (includeAttachements)
                {
                    // TODO: If the "attachment" property of a GET Statement is used and is set to true, the LRS MUST use the multipart response format and include all Attachments as described in Part Two.
                    // Include attachment data, and return mutlipart/form-data
                    Response.ContentType = MIMETypes.Multipart.Mixed;
                    var data = new MultipartContent();
                    data.Add(new StringContent(result.ToJson(), Encoding.UTF8, "application/json"));
                    foreach(var attachment in attachments)
                    {
                        var attachmentContent = new ByteArrayContent(attachment.Content);
                        attachmentContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                        data.Add(attachmentContent);
                    }
                    return Ok(data);
                }

                //var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = new StringContent(result.ToJson(), Encoding.UTF8, MIMETypes.Application.Json);
                return Ok(result);
            }
            catch (Exception e)
            {
                //LogHelper.Error<StatementsController>("GetStatements", e);
                //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
                return BadRequest(e);
            }
        }

        private IActionResult GetStatement([FromQuery]Guid? statementId, [FromQuery]Guid? voidedStatementId)
        {
            if (statementId.HasValue && voidedStatementId.HasValue)
                return BadRequest();

            Guid id = statementId ?? voidedStatementId.Value;
            Statement statement = this._statementService.GetStatement(id, voidedStatementId.HasValue);

            if (statement == null)
                return NotFound();

            Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));
            return Ok(statement);
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult<Guid[]> PostStatements(StatementsPostContent model)
        {
            try
            {
                //Validate(statements);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _logger.LogDebug("Saving statements \n\r {0}", JsonConvert.SerializeObject(model.Statements));
                var ids = this._statementService.SaveStatements(CurrentAuthority, model.Statements);

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString(Constants.Formats.DateTimeFormat));
                return Ok(ids);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Failed to save statements \n\r {0}", JsonConvert.SerializeObject(model.Statements));
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Stores a single Statement with the given id.
        /// </summary>
        /// <param name="statementId"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult PutStatement([FromQuery]Guid statementId, [ModelBinder(typeof(StatementModelBinder))]Statement statement)
        {
            try
            {
                //string body = await Request.Content.ReadAsStringAsync();
                //var statement = JsonConvert.DeserializeObject<Statement>(body);

                if (this._statementService.Exist(statementId))
                    return Conflict(ModelState);

                if (statement == null)
                    throw new ArgumentNullException("statement");

                statement.Authority = CurrentAuthority;
                _statementService.SaveStatement(statement);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
