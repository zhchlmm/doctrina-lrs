using Doctrina.Core.Data;
using Doctrina.Core.Models;
using Doctrina.Core.Services;
using Doctrina.Web.Areas.xAPI.Models;
using Doctrina.Web.Areas.xAPI.Mvc.Filters;
using Doctrina.Web.Mvc.ModelBinders;
using Doctrina.xAPI;
using Doctrina.xAPI.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        [HeadWithoutBody]
        [AcceptVerbs("GET", "HEAD")]
        [Produces("application/json", "multipart/mixed")]
        public async Task<IActionResult> GetStatements([FromQuery]PagedStatementsQuery parameters)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (parameters == null)
                parameters = new PagedStatementsQuery();

            if (parameters.StatementId.HasValue)
                return await GetStatement(parameters.StatementId.Value);

            if (parameters.VoidedStatementId.HasValue)
                return await GetVoidedStatement(parameters.VoidedStatementId.Value);

            try
            {
                StatementsResult result = new StatementsResult();
                int totalCount = 0;
                var statementEntities = this._statementService.GetStatements(parameters, out totalCount);

                // Derserialize to json statement object
                result.Statements = statementEntities
                    .Select(x => JsonConvert.DeserializeObject<Statement>(x.FullStatement))
                    .ToArray();

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

                // Generate more url
                if (result.Statements != null && parameters.Limit.HasValue)
                {
                    parameters.Skip = (parameters.Skip.Value + parameters.Limit.Value);
                    if (parameters.Skip.Value < totalCount)
                    {
                        string parameterMap = parameters.ToParameterMap(parameters.Version).ToString();
                        result.More = new Uri(Url.Action("GetStatements") + "?" + parameterMap, UriKind.Relative);
                    }
                }

                bool includeAttachements = parameters.Attachments.GetValueOrDefault();
                if (includeAttachements)
                {
                    // TODO: If the "attachment" property of a GET Statement is used and is set to true, the LRS MUST use the multipart response format and include all Attachments as described in Part Two.
                    // Include attachment data, and return mutlipart/mixed
                    Response.ContentType = MediaTypes.Multipart.Mixed;
                    var attachmentsWithPayload = statementEntities.SelectMany(x => x.Attachments.Where(a => a.Content != null));

                    var multipart = new MultipartContent("mixed")
                    {
                        new StringContent(result.ToJson(), Encoding.UTF8, MediaTypes.Application.Json)
                    };
                    foreach (var attachment in attachmentsWithPayload)
                    {
                        var byteArrayContent = new ByteArrayContent(attachment.Content);
                        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                        byteArrayContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                        byteArrayContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA2);
                        multipart.Add(byteArrayContent);
                    }
                    var strMultipartContent = await multipart.ReadAsStringAsync();
                    return Content(strMultipartContent, MediaTypes.Multipart.Mixed);
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

        [HttpGet]
        [Produces("application/json", "multipart/mixed")]
        private async Task<IActionResult> GetStatement(
            [FromQuery]Guid statementId,
            [FromQuery(Name = "attachments")]bool includedAttachments = false,
            [FromQuery]ResultFormats format = ResultFormats.Exact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                StatementEntity entity = this._statementService.GetStatement(statementId, false, includedAttachments);

                // TODO: Are we doing it right?
                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

                if (entity == null)
                    return NotFound();

                if (includedAttachments && entity.Attachments.Any(x=> x.Content != null))
                {
                    var multipart = new MultipartContent("mixed")
                    {
                        new StringContent(entity.FullStatement, Encoding.UTF8, MediaTypes.Application.Json)
                }   ;
                    foreach (var attachment in entity.Attachments)
                    {
                        if(attachment.Content != null)
                        {
                            var byteArrayContent = new ByteArrayContent(attachment.Content);
                            byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                            byteArrayContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                            byteArrayContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA2);
                            multipart.Add(byteArrayContent);
                        }
                    }
                    var strMultipart = await multipart.ReadAsStringAsync();
                    return Content(strMultipart, MediaTypes.Multipart.Mixed);
                }

                return Content(entity.FullStatement, MediaTypes.Application.Json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStatement");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json", "multipart/mixed")]
        private async Task<IActionResult> GetVoidedStatement(
            [FromQuery]Guid voidedStatementId, 
            [FromQuery(Name="attachments")]bool includeAttachments = false, 
            [FromQuery]ResultFormats format = ResultFormats.Exact)
        {
            try
            {
                StatementEntity entity = this._statementService.GetStatement(voidedStatementId, true, includeAttachments);

                // TODO: Are we doing it right?
                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

                if (entity == null)
                    return NotFound();

                // TODO: Deserialize based on format
                //var version = XAPIVersion.Latest();
                //XAPISerializer xserializer = new XAPISerializer(version, format);
                //xserializer.Deserialize()

                if (includeAttachments && entity.Attachments.Any(x=> x.Content != null))
                {
                    var multipart = new MultipartContent("mixed")
                    {
                        new StringContent(entity.FullStatement, Encoding.UTF8, MediaTypes.Application.Json)
                    };
                    foreach (var attachment in entity.Attachments)
                    {
                        if(attachment.Content != null)
                        {
                            var byteArrayContent = new ByteArrayContent(attachment.Content);
                            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                            byteArrayContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                            byteArrayContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA2);
                            multipart.Add(byteArrayContent);
                        }
                    }

                    return Content(await multipart.ReadAsStringAsync(), MediaTypes.Multipart.Mixed);
                }
             
                return Content(entity.FullStatement, MediaTypes.Application.Json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVoidedStatement");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Order = 2)]
        [Produces("application/json")]
        public ActionResult<Guid[]> PostStatements(StatementsPostContent model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Guid[] ids = _statementService.CreateStatements(Authority, model.Statements);

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
        [VersionHeader]
        [HttpPut]
        public IActionResult PutStatement([FromQuery]Guid statementId, [ModelBinder(typeof(StatementModelBinder))]Statement statement)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                statement.Id = statementId;
                statement.Authority = Authority;
                var savedEntity = this._statementService.GetStatement(statementId);
                if (savedEntity != null)
                {
                    var savedStatement = JsonConvert.DeserializeObject<Statement>(savedEntity.FullStatement);
                    if (savedStatement.Equals(statement))
                    {
                        return NoContent();
                    }
                    return Conflict();
                }

                if (statement == null)
                    throw new ArgumentNullException("statement");

                _statementService.CreateStatements(Authority, statement);

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
