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
        private readonly IAttachmentService _attachmentService;
        private readonly ILogger<StatementsController> _logger;

        public StatementsController(IStatementService statementService, IAttachmentService attachmentService, ILogger<StatementsController> logger)
        {
            _statementService = statementService;
            _attachmentService = attachmentService;
            _logger = logger;
        }

        [HttpGet]
        [Produces("application/json", "multipart/mixed")]
        public IActionResult GetStatements([FromQuery]PagedStatementsQuery parameters, 
            [FromHeader(Name = Constants.Headers.XExperienceApiVersion)]string version)
        {
            if (parameters == null)
                parameters = new PagedStatementsQuery();

            if (parameters.StatementId.HasValue)
                return GetStatement(parameters);

            if (parameters.VoidedStatementId.HasValue)
                return GetVoidedStatement(parameters.VoidedStatementId.Value);
            
            try
            {
                StatementsResult result = new StatementsResult();
                int totalCount = 0;
                result.Statements = this._statementService.GetStatements(parameters, out totalCount);

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
                    var attachments = new List<AttachmentEntity>();
                    foreach(var stmt in result.Statements)
                    {
                        if(stmt.Attachments != null)
                        {
                            foreach (var att in stmt.Attachments)
                            {
                            }
                        }
                    }
                    // TODO: If the "attachment" property of a GET Statement is used and is set to true, the LRS MUST use the multipart response format and include all Attachments as described in Part Two.
                    // Include attachment data, and return mutlipart/form-data
                    Response.ContentType = MediaTypes.Multipart.Mixed;

                    var mixed = new MultipartContent("mixed")
                    {
                        new StringContent(result.ToJson(), Encoding.UTF8, MediaTypes.Application.Json)
                    };
                    foreach (var attachment in attachments)
                    {
                        var byteArrayContent = new ByteArrayContent(attachment.Content);
                        byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                        byteArrayContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                        byteArrayContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA2);
                        mixed.Add(byteArrayContent);
                    }

                    return Ok(mixed);
                }

                //var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = new StringContent(result.ToJson(), Encoding.UTF8, MIMETypes.Application.Json);
                Response.ContentType = MediaTypes.Application.Json;
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStatements");
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        //[Produces("application/json", "multipart/mixed")]
        //private IActionResult GetStatement([FromQuery]Guid statementId)
        //{
        //    Response.ContentType = MediaTypes.Application.Json;

        //    // Contains other parameters?
        //    if (parameters.Agent != null
        //        || !string.IsNullOrEmpty(parameters.ActivityId)
        //        || parameters.Ascending.HasValue
        //        || parameters.Limit.HasValue
        //        || parameters.Registration.HasValue
        //        || parameters.RelatedActivities.HasValue
        //        || parameters.RelatedAgents.HasValue
        //        || parameters.Since.HasValue
        //        || parameters.Until.HasValue
        //        || parameters.VerbId != null)
        //    {
        //        if(parameters.VoidedStatementId.HasValue)
        //            ModelState.AddModelError("voidedStatementId", "May only be paired with 'format' and 'attachments' paramaters.");

        //        if (parameters.StatementId.HasValue)
        //            ModelState.AddModelError("statementId", "May only be paired with 'format' and 'attachments' paramaters.");
        //    }

        //    if (parameters.StatementId.HasValue && parameters.VoidedStatementId.HasValue)
        //    {
        //        ModelState.AddModelError("statementId", "Cannot be paired with 'voidedStatementId'.");
        //        ModelState.AddModelError("voidedStatementId", "Cannot be paired with 'statementId'.");
        //    }

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    try
        //    {
        //        Guid id = parameters.StatementId ?? parameters.VoidedStatementId.Value;
        //        Statement statement = this._statementService.GetStatement(id, parameters.VoidedStatementId.HasValue);

        //        if (statement == null)
        //            return NotFound();

        //        Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));
        //        return Ok(statement);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "GetStatement");
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        [Produces("application/json", "multipart/mixed")]
        private IActionResult GetVoidedStatement(
            [FromQuery]Guid voidedStatementId, 
            [FromQuery]bool? attachments = null, 
            [FromQuery]string format = null)
        {
            try
            {
                Statement statement = this._statementService.GetStatement(voidedStatementId, true);

                if (statement == null)
                    return NotFound();

                if (attachments.GetValueOrDefault())
                {
                    if(statement.Attachments.Length > 0)
                    {

                    }
                    //var attachments = _attachmentService.GetAttachments(statement.Id.Value);
                    // TODO: Return multipart mixed with attachments
                }
             
                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));
                return Ok(statement);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVoidedStatement");
                return BadRequest(ex.Message);
            }
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
                var ids = _statementService.CreateStatements(CurrentAuthority, model.Statements);

                // TODO: Save attachments


                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString(Constants.Formats.DateTimeFormat));
                return Ok(ids);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostStatements");
                return BadRequest(ex.Message);
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

                _statementService.CreateStatements(CurrentAuthority, statement);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PutStatement: {0}", statementId);
                return BadRequest(ex.Message);
            }
        }
    }
}
