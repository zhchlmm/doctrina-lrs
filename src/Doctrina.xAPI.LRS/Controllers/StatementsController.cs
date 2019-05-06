using Doctrina.Persistence.Entities;
using Doctrina.Persistence.Models;
using Doctrina.Persistence.Services;
using Doctrina.WebUI.Mvc.ModelBinders;
using Doctrina.xAPI.Exceptions;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.LRS.Models;
using Doctrina.xAPI.LRS.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Doctrina.xAPI.LRS.Controllers
{
    /// <summary>
    /// The basic communication mechanism of the Experience API.
    /// </summary>
    //[LRSAuthortize]
    //[ApiVersion]
    [RequiredVersionHeaderAttribute]
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

        [HttpGet(Order = 1)]
        [Produces("application/json", "multipart/mixed")]
        private async Task<IActionResult> GetStatement(
            [FromQuery]Guid statementId,
            [FromQuery(Name = "attachments")]bool includeAttachments = false,
            [FromQuery]ResultFormats format = ResultFormats.Exact)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                StatementEntity entity = this._statementService.GetStatement(statementId, false, includeAttachments);

                if (entity == null)
                    return NotFound();

                if (includeAttachments && entity.Attachments.Any(x => x.Content != null))
                {
                    var multipart = new MultipartContent("mixed")
                        {
                            new StringContent(entity.FullStatement, Encoding.UTF8, MediaTypes.Application.Json)
                    };
                    foreach (var attachment in entity.Attachments)
                    {
                        if (attachment.Content != null)
                        {
                            var byteArrayContent = new ByteArrayContent(attachment.Content);
                            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                            byteArrayContent.Headers.Add(Headers.ContentTransferEncoding, "binary");
                            byteArrayContent.Headers.Add(Headers.XExperienceApiHash, attachment.SHA2);
                            multipart.Add(byteArrayContent);
                        }
                    }
                    var strMultipart = await multipart.ReadAsStringAsync();
                    return Content(strMultipart, MediaTypes.Multipart.Mixed);
                }

                return Content(entity.FullStatement, MediaTypes.Application.Json);

            }
            catch (RequirementException ex)
            {
                _logger.LogWarning(ex, "GetStatement");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStatement");
                throw ex;
            }
        }

        [HttpGet(Order = 2)]
        [Produces("application/json", "multipart/mixed")]
        private async Task<IActionResult> GetVoidedStatement(
            [FromQuery]Guid voidedStatementId,
            [FromQuery(Name = "attachments")]bool includeAttachments = false,
            [FromQuery]ResultFormats format = ResultFormats.Exact)
        {
            try
            {
                StatementEntity entity = this._statementService.GetStatement(voidedStatementId, true, includeAttachments);

                if (entity == null)
                    return NotFound();

                if (includeAttachments && entity.Attachments.Any(x => x.Content != null))
                {
                    var multipart = new MultipartContent("mixed")
                    {
                        new StringContent(entity.FullStatement, Encoding.UTF8, MediaTypes.Application.Json)
                    };
                    foreach (var attachment in entity.Attachments)
                    {
                        if (attachment.Content != null)
                        {
                            var byteArrayContent = new ByteArrayContent(attachment.Content);
                            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                            byteArrayContent.Headers.Add(Headers.ContentTransferEncoding, "binary");
                            byteArrayContent.Headers.Add(Headers.XExperienceApiHash, attachment.SHA2);
                            multipart.Add(byteArrayContent);
                        }
                    }

                    return Content(await multipart.ReadAsStringAsync(), MediaTypes.Multipart.Mixed);
                }

                return Content(entity.FullStatement, MediaTypes.Application.Json);
            }
            catch (RequirementException ex)
            {
                _logger.LogWarning(ex, "GetVoidedStatement");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVoidedStatement");
                throw ex;
            }
        }

        /// <summary>
        /// Get statements
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "HEAD", Order = 3)]
        [Produces("application/json", "multipart/mixed")]
        public async Task<IActionResult> GetStatements([FromQuery]PagedStatementsQuery parameters)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (parameters == null)
                    parameters = new PagedStatementsQuery();

                // Validate parameters for combinations
                if (parameters.StatementId.HasValue || parameters.VoidedStatementId.HasValue)
                {
                    var otherParameters = parameters.ToParameterMap(ApiVersion.GetLatest());
                    otherParameters.Remove("attachments");
                    otherParameters.Remove("format");
                    if (otherParameters.Count > 0)
                    {
                        return BadRequest("Only attachments and format parameters are allowed with using statementId or voidedStatementId");
                    }

                    bool attachments = parameters.Attachments.GetValueOrDefault();
                    ResultFormats format = parameters.Format ?? ResultFormats.Exact;

                    if (parameters.StatementId.HasValue)
                        return await GetStatement(
                            parameters.StatementId.Value,
                            attachments,
                            format);

                    if (parameters.VoidedStatementId.HasValue)
                        return await GetVoidedStatement(
                            parameters.VoidedStatementId.Value,
                            attachments,
                            format);
                }

                StatementsResult result = new StatementsResult();
                int totalCount = 0;
                var statementEntities = this._statementService.GetStatements(parameters, out totalCount);

                // Derserialize to json statement object
                result.Statements = statementEntities
                    .Select(x => JsonConvert.DeserializeObject<Statement>(x.FullStatement))
                    .ToArray();

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
                    return await MultipartResult(result, statementEntities);
                }

                //var response = Request.CreateResponse(HttpStatusCode.OK);
                //response.Content = new StringContent(result.ToJson(), Encoding.UTF8, MIMETypes.Application.Json);
                Response.ContentType = MediaTypes.Application.Json;

                return Ok(result);
            }
            catch (RequirementException ex)
            {
                _logger.LogWarning(ex, "GetStatements");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStatements");
                throw ex;
            }
        }

        private async Task<IActionResult> MultipartResult(object result, IEnumerable<StatementEntity> statementEntities)
        {
            Response.ContentType = MediaTypes.Multipart.Mixed;
            var attachmentsWithPayload = statementEntities.SelectMany(x => x.Attachments.Where(a => a.Content != null));
            string jsonString = JsonConvert.SerializeObject(result);

            var multipart = new MultipartContent("mixed");
            multipart.Add(new StringContent(jsonString, Encoding.UTF8, MediaTypes.Application.Json));

            foreach (var attachment in attachmentsWithPayload)
            {
                var byteArrayContent = new ByteArrayContent(attachment.Content);
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                byteArrayContent.Headers.Add(Headers.ContentTransferEncoding, "binary");
                byteArrayContent.Headers.Add(Headers.XExperienceApiHash, attachment.SHA2);
                multipart.Add(byteArrayContent);
            }
            var strMultipartContent = await multipart.ReadAsStringAsync();
            return Content(strMultipartContent, MediaTypes.Multipart.Mixed);
        }

        /// <summary>
        /// Create statement(s) with attachment(s)
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Array of Statement id(s) (UUID) in the same order as the corresponding stored Statements.</returns>
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

                var ids = new List<Guid>();
                var statements = new List<StatementEntity>();
                foreach (var statement in model.Statements)
                {
                    if (statement.Authority == null)
                    {
                        statement.Authority = Authority;
                    }
                    var entity = _statementService.CreateStatement(statement);
                    statements.Add(entity);
                    ids.Add(entity.StatementId);
                }

                _statementService.Save(statements.ToArray());

                return Ok(ids.ToArray());
            }
            catch (RequirementException ex)
            {
                _logger.LogWarning(ex, "PostStatements");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PostStatements");
                throw ex;
            }
        }

        /// <summary>
        /// Stores a single Statement with the given id.
        /// </summary>
        /// <param name="statementId"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        [RequiredVersionHeader]
        [HttpPut]
        public IActionResult PutStatement([FromQuery]Guid statementId, [ModelBinder(typeof(StatementPutModelBinder))]Statement statement)
        {
            if (statementId.Equals(Guid.Empty))
            {
                return BadRequest(new ArgumentNullException(nameof(statementId)));
            }

            if (statement == null)
                return BadRequest(new ArgumentNullException(nameof(statementId)));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                statement.Id = statementId;
                statement.Authority = Authority; // TODO: Validate authority
                var savedEntity = this._statementService.GetStatement(statementId);
                if (savedEntity != null)
                {
                    var savedStatement = JsonConvert.DeserializeObject<Statement>(savedEntity.FullStatement);
                    if (!savedStatement.Equals(statement))
                    {
                        return Conflict();
                    }
                }
                else
                {
                    var entity = _statementService.CreateStatement(statement);
                    _statementService.Save(entity);
                }

                return NoContent();
            }
            catch (RequirementException ex)
            {
                _logger.LogWarning(ex, "PutStatement: {0}", statementId);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PutStatement: {0}", statementId);
                throw ex;
            }
        }
    }
}
