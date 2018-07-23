using Doctrina.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using UmbracoLRS.Core.ModelBinders;

namespace Doctrina.Web.Controllers
{
    /// <summary>
    /// The basic communication mechanism of the Experience API.
    /// </summary>
    [ApiController]
    [LRSAuthortize]
    [ApiVersion]
    [Produces("application/json", "multipart/mixed")]
    [Route("api/statements")]
    public class StatementsController : ApiController
    {
        private readonly IStatementService statementService;

        public StatementsController(StatementService statementService)
        {
            this.statementService = statementService;
        }

        public BaseSearchProvider LRSSearcher
        {
            get
            {
                return ExamineManager.Instance.SearchProviderCollection[Constants.LRSSearchProvider];
            }
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetStatement([FromUri]Guid? statementId, [FromUri]Guid? voidedStatementId)
        {
            if (statementId.HasValue && voidedStatementId.HasValue)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            Statement statement = null;
            if (statementId.HasValue)
            {
                statement = this.StatementService.GetStatement(statementId.Value, voided: false);
            }
            else if (voidedStatementId.HasValue)
            {
                statement = this.StatementService.GetStatement(voidedStatementId.Value, voided: true);
            }

            var response = new HttpResponseMessage();
            if (statement == null)
                response = Request.CreateResponse(HttpStatusCode.NotFound);
            else
                response = Request.CreateResponse(HttpStatusCode.OK, statement);

            response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));
            return response;
        }

        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetStatements([FromUri]StatementsQuery parameters)
        {
            LogHelper.Debug<StatementsController>("GetStatements \r\n{0}", () => Request.RequestUri.ToString());

            if (parameters == null)
                parameters = new StatementsQuery();

            if (parameters.StatementId.HasValue || parameters.VoidedStatementId.HasValue)
                return GetStatement(parameters.StatementId, parameters.VoidedStatementId);

            try
            {
                bool includeAttachements = parameters.Attachments.GetValueOrDefault();
                StatementsResult result = new StatementsResult();
                var pagedResult = this.StatementService.GetStatements(parameters);
                result.Statements = pagedResult.Items;

                // Generate more url
                if (pagedResult.TotalPages > pagedResult.PageNumber)
                {
                    string authority = Request.RequestUri.GetLeftPart(UriPartial.Authority);
                    string path = Request.RequestUri.AbsolutePath;

                    parameters.Page += 1;
                    string query = parameters.ToParameterMap(XAPIVersion.V103).ToString();
                    result.More = new Uri($"{path}?{query}", UriKind.Relative);
                }

                if (includeAttachements)
                {
                    // TODO: If the "attachment" property of a GET Statement is used and is set to true, the LRS MUST use the multipart response format and include all Attachments as described in Part Two.
                    // Include attachment data, and return mutlipart/form-data
                    return Request.CreateResponse(HttpStatusCode.OK, result, MIMETypes.Multipart.Mixed);
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(result.ToJson(), Encoding.UTF8, MIMETypes.Application.Json);
                response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));
                return response;
            }
            catch (Exception e)
            {
                LogHelper.Error<StatementsController>("GetStatements", e);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }

        }

        //[HttpGet]
        //[Route("{continueToken:Guid}")]
        //public HttpResponseMessage GetMoreStatements(Guid key)
        //{
        //    // TODO: Get Query by key
        //    //var statementQuery = this.StatementService.GetStatementQuery(key);
        //    //return GetStatements(statementQuery, );
        //    throw new NotImplementedException();
        //}

        [HttpPost]
        [Route("")]
        public HttpResponseMessage PostStatements([ModelBinder(typeof(StatementsModelBinder))]Statement[] statements)
        {
            try
            {
                Validate(statements);

                if (!ModelState.IsValid)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                var authority = new Agent()
                {
                    Name = "Admin",
                    OpenId = new IRI(Request.RequestUri.GetLeftPart(UriPartial.Authority))
                };
                var ids = this.StatementService.StoreStatements(authority, statements);

                var response = Request.CreateResponse(HttpStatusCode.OK, ids);
                response.Headers.Add(Constants.Headers.ConsistentThrough, DateTime.UtcNow.ToString("o"));
                return response;
            }
            catch (Exception e)
            {
                //LogHelper.Debug<StatementsController>("PostStatements \r\n {0}", () => JsonConvert.SerializeObject(statements, Formatting.Indented));
                LogHelper.Error<StatementsController>("PostStatements", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Stores a single Statement with the given id.
        /// </summary>
        /// <param name="statementId"></param>
        /// <param name="statement"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<HttpResponseMessage> PutStatement(Guid statementId, [ModelBinder(typeof(StatementModelBinder))]Statement statement)
        {
            try
            {
                //string body = await Request.Content.ReadAsStringAsync();
                //var statement = JsonConvert.DeserializeObject<Statement>(body);

                if (StatementService.StatementExist(statementId))
                    return Request.CreateResponse(HttpStatusCode.Conflict);

                if (statement == null)
                    throw new ArgumentNullException("statement");

                var authority = new Agent()
                {
                    Name = "Admin",
                    OpenId = new IRI(Request.RequestUri.GetLeftPart(UriPartial.Authority))
                };

                statement.Authority = authority;
                StatementService.CreateStatement(statement);

                await Task.FromResult(0);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
