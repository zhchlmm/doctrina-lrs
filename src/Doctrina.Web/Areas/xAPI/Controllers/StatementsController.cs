using Doctrina.Core.Data;
using Doctrina.Core.Models;
using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.Web.Mvc.ModelBinders;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        public IActionResult GetStatements([FromQuery]PagedStatementsQuery parameters, 
            [FromHeader(Name = Constants.Headers.XExperienceApiVersion)]string version)
        {
            if (parameters == null)
                parameters = new PagedStatementsQuery();

            if (parameters.StatementId.HasValue || parameters.VoidedStatementId.HasValue)
                return GetStatement(parameters.StatementId, parameters.VoidedStatementId);

            try
            {
                StatementsResult result = new StatementsResult();
                int totalCount = 0;
                result.Statements = this._statementService.GetStatements(parameters, out totalCount);
                var attachments = new List<AttachmentEntity>();

                // Generate more url
                if (result.Statements != null && parameters.Limit.HasValue)
                {
                    parameters.Skip = (parameters.Skip.Value + parameters.Limit.Value);
                    if(parameters.Skip.Value < totalCount)
                    {
                        string parameterMap = parameters.ToParameterMap(version).ToString();
                        result.More = new Uri(Url.Action("GetStatements") + "?" + parameterMap, UriKind.Relative);
                    }
                }

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

                bool includeAttachements = parameters.Attachments.GetValueOrDefault();
                if (includeAttachements)
                {
                    // TODO: If the "attachment" property of a GET Statement is used and is set to true, the LRS MUST use the multipart response format and include all Attachments as described in Part Two.
                    // Include attachment data, and return mutlipart/form-data
                    Response.ContentType = MediaTypes.Multipart.Mixed;

                    var mixed = new MultipartContent("mixed"); // TODO: missing boundary?
                    mixed.Add(new StringContent(result.ToJson(), Encoding.UTF8, MediaTypes.Application.Json));

                    foreach(var attachment in attachments)
                    {
                        var attachmentContent = new ByteArrayContent(attachment.Content);
                        attachmentContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                        attachmentContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                        attachmentContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA);
                        mixed.Add(attachmentContent);
                    }

                    return Ok(mixed);
                }

                //var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = new StringContent(result.ToJson(), Encoding.UTF8, MIMETypes.Application.Json);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStatements");
                return BadRequest(ex);
            }
        }

        private IActionResult GetStatement([FromQuery]Guid? statementId, [FromQuery]Guid? voidedStatementId)
        {
            Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

            if (statementId.HasValue && voidedStatementId.HasValue)
                return BadRequest();

            Guid id = statementId ?? voidedStatementId.Value;
            Statement statement = this._statementService.GetStatement(id, voidedStatementId.HasValue);

            if (statement == null)
                return NotFound();

            return Ok(statement);
        }

        [HttpPost]
        [Produces("application/json")]
        public ActionResult<Guid[]> PostStatements(StatementsPostContent model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var ids = _statementService.SaveStatements(CurrentAuthority, model.Statements);

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString(Constants.Formats.DateTimeFormat));
                return Ok(ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostStatements");
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
                statement.Id = statementId;
                statement.Authority = CurrentAuthority;
                var saved = this._statementService.GetStatement(statementId);
                if (saved != null)
                {
                    if (saved.Equals(statement))
                    {
                        return NoContent();
                    }
                    return Conflict();
                }

                if (statement == null)
                    throw new ArgumentNullException("statement");

                _statementService.SaveStatements(CurrentAuthority, statement);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PutStatement: {0}", statementId);
                return BadRequest(ex);
            }
        }
    }
}
