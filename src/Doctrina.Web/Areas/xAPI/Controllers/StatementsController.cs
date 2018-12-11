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

        [HttpGet]
        [Produces("application/json", "multipart/mixed")]
        public IActionResult GetStatements([FromQuery]PagedStatementsQuery parameters)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (parameters == null)
                parameters = new PagedStatementsQuery();

            if (parameters.StatementId.HasValue)
                return GetStatement(parameters.StatementId.Value);

            if (parameters.VoidedStatementId.HasValue)
                return GetVoidedStatement(parameters.VoidedStatementId.Value);

            try
            {
                StatementsResult result = new StatementsResult();
                int totalCount = 0;
                var statementEntities = this._statementService.GetStatements(parameters, out totalCount);

                // Derserialize to json statement object
                result.Statements = statementEntities.Select(x => JsonConvert.DeserializeObject<Statement>(x.FullStatement));

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

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

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
                        byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                        byteArrayContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                        byteArrayContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA2);
                        multipart.Add(byteArrayContent);
                    }

                    return Ok(multipart);
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
        private IActionResult GetStatement(
            [FromQuery]Guid statementId,
            [FromQuery(Name = "attachments")]bool includedAttachments = false,
            [FromQuery]ResultFormats format = ResultFormats.Exact)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                StatementEntity entity = this._statementService.GetStatement(statementId, false, includedAttachments);

                if (entity == null)
                    return NotFound();

                var jsonObject = new StringContent(entity.FullStatement, Encoding.UTF8, MediaTypes.Application.Json);

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

                if (includedAttachments)
                {
                    var multipart = new MultipartContent("mixed")
                    {
                        jsonObject
                    };
                    foreach (var attachment in entity.Attachments)
                    {
                        var byteArrayContent = new ByteArrayContent(attachment.Content);
                        byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                        byteArrayContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                        byteArrayContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA2);
                        multipart.Add(byteArrayContent);
                    }
                    return Ok(multipart);
                }

                return Ok(jsonObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetStatement");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Produces("application/json", "multipart/mixed")]
        private IActionResult GetVoidedStatement(
            [FromQuery]Guid voidedStatementId, 
            [FromQuery(Name="attachments")]bool includeAttachments = false, 
            [FromQuery]ResultFormats format = ResultFormats.Exact)
        {
            try
            {
                StatementEntity entity = this._statementService.GetStatement(voidedStatementId, true, includeAttachments);

                if (entity == null)
                    return NotFound();

                // TODO: Deserialize based on format
                //var version = XAPIVersion.Latest();
                //XAPISerializer xserializer = new XAPISerializer(version, format);
                //xserializer.Deserialize()
                var jsonObject = new StringContent(entity.FullStatement, Encoding.UTF8, MediaTypes.Application.Json);

                Response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));

                if (includeAttachments)
                {
                    var multipart = new MultipartContent("mixed")
                    {
                        jsonObject
                    };
                    foreach (var attachment in entity.Attachments)
                    {
                        var byteArrayContent = new ByteArrayContent(attachment.Content);
                        byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                        byteArrayContent.Headers.Add(Constants.Headers.ContentTransferEncoding, "binary");
                        byteArrayContent.Headers.Add(Constants.Headers.XExperienceApiHash, attachment.SHA2);
                        multipart.Add(byteArrayContent);
                    }
                    return Ok(multipart);
                    //var attachments = _attachmentService.GetAttachments(statement.Id.Value);
                    // TODO: Return multipart mixed with attachments
                }
             
                return Ok(jsonObject);
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
        [HttpPut]
        public IActionResult PutStatement([FromQuery]Guid statementId, [ModelBinder(typeof(StatementModelBinder))]Statement statement)
        {
            try
            {
                statement.Id = statementId;
                statement.Authority = Authority;
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

                _statementService.CreateStatements(Authority, statement);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PutStatement: {0}", statementId);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Alternate Request Syntax
        /// </summary>
        /// <param name="method">The intended xAPI method.</param>
        /// <param name="content">UTF-8 string</param>
        /// <returns></returns>
        [HttpPost(Order = 1)]
        public async Task<IActionResult> AlternateRequest(string method)
        {
            var methodNames = new string[] { "POST", "GET", "PUT", "DELETE" };
            if (!methodNames.Contains(method.ToUpperInvariant()))
            {
                ModelState.AddModelError(nameof(method), "Not a valid HTTP method.");
            }

            var formData = Request.Form.ToDictionary(x=> x.Key, y => y.Value);

            // Get content data
            HttpContent httpContent = new StringContent("");
            if (formData.ContainsKey("content"))
            {

                string encodedContent = formData["content"];
                formData.Remove("content");
                httpContent = new StringContent(encodedContent);
            }

            var headerNames = new string[] { "Authorization", "X-Experience-API-Version", "Content-Type", "Content-Length", "If-Match", "If-None-Match" };
            var requestHeaders = new Dictionary<string, string>();
            foreach(var name in headerNames)
            {
                if (formData.ContainsKey(name))
                {
                    requestHeaders.Add(name, formData[name]);
                    formData.Remove(name);
                }
            }

            foreach(var header in Request.Headers)
            {
                requestHeaders.Add(header.Key, header.Value);
            }

            // Treat all other parameters and queryString paramaters
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var name in formData)
            {
                queryString.Add(name.Key, name.Value);
                formData.Remove(name.Key);
            }

            string requestUri = Request.Scheme + "://" + Request.Host + "/xapi/statements";
            if(queryString.Count > 0)
            {
                requestUri += "?" + queryString.ToString();
            }

            // Return response from intended xAPI method.
            using (HttpClient client = new HttpClient())
            {
                var response = new HttpResponseMessage();
                foreach(var header in requestHeaders)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                switch (method)
                {
                    case "GET":
                        response = await client.GetAsync(requestUri);
                        break;
                    case "DELETE":
                        response = await client.DeleteAsync(requestUri);
                        break;
                    case "POST":
                        response = await client.PostAsync(requestUri, httpContent);
                        break;
                    case "PUT":
                        response = await client.PutAsync(requestUri, httpContent);
                        break;
                }

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(response.Content);
                }

                string contentType = response.Headers.GetValues("Content-Type").FirstOrDefault();

                return Ok(response.Content);
            }
        }
    }
}
