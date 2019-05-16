using Doctrina.Application.Statements.Commands;
using Doctrina.Application.Statements.Queries;
using Doctrina.WebUI.Mvc.ModelBinders;
using Doctrina.xAPI.Http;
using Doctrina.xAPI.LRS.Models;
using Doctrina.xAPI.LRS.Mvc.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IMediator _mediator;
        private readonly ILogger<StatementsController> _logger;

        public StatementsController(IMediator mediator, ILogger<StatementsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet(Order = 1)]
        [Produces("application/json", "multipart/mixed")]
        private async Task<IActionResult> GetStatement(
            [FromQuery]Guid statementId,
            [FromQuery(Name = "attachments")]bool includeAttachments = false,
            [FromQuery]ResultFormat format = ResultFormat.Exact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Statement statement = await _mediator.Send(GetStatementQuery.Create(statementId, includeAttachments, format));

            if (statement == null)
                return NotFound();

            string fullStatement = statement.ToJson();

            if (includeAttachments && statement.Attachments.Any(x => x.Payload != null))
            {
                var multipart = new MultipartContent("mixed")
                    {
                        new StringContent(fullStatement, Encoding.UTF8, MediaTypes.Application.Json)
                    };
                foreach (var attachment in statement.Attachments)
                {
                    if (attachment.Payload != null)
                    {
                        var byteArrayContent = new ByteArrayContent(attachment.Payload);
                        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                        byteArrayContent.Headers.Add(Headers.ContentTransferEncoding, "binary");
                        byteArrayContent.Headers.Add(Headers.XExperienceApiHash, attachment.SHA2);
                        multipart.Add(byteArrayContent);
                    }
                }
                var strMultipart = await multipart.ReadAsStringAsync();
                return Content(strMultipart, MediaTypes.Multipart.Mixed);
            }

            return Content(fullStatement, MediaTypes.Application.Json);

        }

        [HttpGet(Order = 2)]
        [Produces("application/json", "multipart/mixed")]
        private async Task<IActionResult> GetVoidedStatement(
            [FromQuery]Guid voidedStatementId,
            [FromQuery(Name = "attachments")]bool includeAttachments = false,
            [FromQuery]ResultFormat format = ResultFormat.Exact)
        {
            Statement statement = await _mediator.Send(GetVoidedStatemetQuery.Create(voidedStatementId, includeAttachments, format));

            if (statement == null)
                return NotFound();

            string fullStatement = statement.ToJson();

            if (includeAttachments && statement.Attachments.Any(x => x.Payload != null))
            {
                var multipart = new MultipartContent("mixed")
                    {
                        new StringContent(fullStatement, Encoding.UTF8, MediaTypes.Application.Json)
                    };
                foreach (var attachment in statement.Attachments)
                {
                    if (attachment.Payload != null)
                    {
                        var byteArrayContent = new ByteArrayContent(attachment.Payload);
                        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(attachment.ContentType);
                        byteArrayContent.Headers.Add(Headers.ContentTransferEncoding, "binary");
                        byteArrayContent.Headers.Add(Headers.XExperienceApiHash, attachment.SHA2);
                        multipart.Add(byteArrayContent);
                    }
                }

                return Content(await multipart.ReadAsStringAsync(), MediaTypes.Multipart.Mixed);
            }

            return Content(fullStatement, MediaTypes.Application.Json);

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
                ResultFormat format = parameters.Format ?? ResultFormat.Exact;

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
            ICollection<Statement> statements = await _mediator.Send(parameters);

            // Derserialize to json statement object
            result.Statements = new StatementCollection(statements);

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
                return await MultipartResult(result, statements);
            }

            //var response = Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(result.ToJson(), Encoding.UTF8, MIMETypes.Application.Json);
            Response.ContentType = MediaTypes.Application.Json;

            return Ok(result);
        }

        private async Task<IActionResult> MultipartResult(JsonModel result, ICollection<Statement> statements)
        {
            Response.ContentType = MediaTypes.Multipart.Mixed;
            var attachmentsWithPayload = statements.SelectMany(x => x.Attachments.Where(a => a.Payload != null));

            var multipart = new MultipartContent("mixed");
            multipart.Add(new StringContent(result.ToJson(), Encoding.UTF8, MediaTypes.Application.Json));

            foreach (var attachment in attachmentsWithPayload)
            {
                var byteArrayContent = new ByteArrayContent(attachment.Payload);
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
        public async Task<ActionResult<ICollection<Guid>>> PostStatements(StatementsPostContent model)
        {
            ICollection<Guid> guids = await _mediator.Send(CreateStatementsCommand.Create(model.Statements));

            return Ok(guids);
        }

        /// <summary>
        /// Stores a single Statement with the given id.
        /// </summary>
        /// <param name="statementId"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        [RequiredVersionHeader]
        [HttpPut]
        public async Task<IActionResult> PutStatement([FromQuery]Guid statementId, [ModelBinder(typeof(StatementPutModelBinder))]Statement statement)
        {
            await _mediator.Send(PutStatementCommand.Create(statementId, statement));

            return NoContent();
        }
    }
}
