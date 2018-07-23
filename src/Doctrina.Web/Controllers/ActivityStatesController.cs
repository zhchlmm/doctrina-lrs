using ExperienceAPI.Core.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using LRS.Core;
using UmbracoLRS.Core.Authentication;
using UmbracoLRS.Core.Http;
using UmbracoLRS.Core.ModelBinders;
using UmbracoLRS.Core.Models;
using UmbracoLRS.Core.Mvc.TypeConverters;
using UmbracoLRS.Core.Services;

namespace UmbracoLRS.Core.Controllers
{
    /// <summary>
    /// Generally, this is a scratch area for Learning Record Providers that do not have their own internal storage, or need to persist state across devices.
    /// </summary>
    [ApiAuthortize]
    [ApiVersion]
    [RoutePrefix("xapi/activities/state")]
    public class ActivityStatesController : ApiController
    {
        private ActivityStateService stateResource;

        protected ActivityStatesController()
        {
            this.stateResource = new ActivityStateService(ApplicationContext.Current);
        }

        // GET xapi/activities/state
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetSingleState(
            StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                var stateDocument = stateResource.GetSingleState(model.ActivityId, model.Agent, model.StateId, model.Registration);


                var result = new HttpResponseMessage(HttpStatusCode.OK);
                if(stateDocument.MimeType == "application/json")
                {
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var jObject = JObject.Parse(stateDocument.JsonDocument);
                    result.Content = new JsonContent(jObject);
                }
                else
                {
                    result.Content.Headers.ContentType = new MediaTypeHeaderValue(stateDocument.MimeType);
                    result.Content = new StreamContent(File.OpenRead(stateDocument.DocumentId));
                }

                result.Headers.ETag = new System.Net.Http.Headers.EntityTagHeaderValue("\"" + stateDocument.ETag + "\"");
                result.Content.Headers.LastModified = stateDocument.Updated;
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT|POST xapi/activities/state
        [HttpPut]
        [HttpPost]
        [Route("")]
        public HttpResponseMessage PostSingleState(
            [ModelBinder(typeof(StateDocumentModelBinder))] StateDocumentModel model)
        {
            // TODO: Implement binary data
            if(model.MediaType != "application/json")
                throw new NotImplementedException();

            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                var state = stateResource.MergeState(model.ActivityId, model.Agent, model.StateId, model.Registration, model.JsonDocument, model.MediaType);

                var response = Request.CreateResponse(HttpStatusCode.NoContent);
                response.Headers.ETag = new EntityTagHeaderValue("\"" + state.ETag + "\"");
                //response.Content.Headers.LastModified = state.Updated; Content is null
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        // DELETE xapi/activities/state
        [HttpDelete]
        [Route("")]
        public HttpResponseMessage DeleteSingleState([FromUri]StateDocumentModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                stateResource.DeleteState(model.ActivityId, model.Agent, model.StateId, model.Registration);

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        /// <summary>
        /// Fetches State ids of all state data for this context (Activity + Agent [ + registration if specified]). If "since" parameter is specified, this is limited to entries that have been stored or updated since the specified timestamp (exclusive).
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="agent"></param>
        /// <param name="stateId"></param>
        /// <param name="registration"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetMutipleStates(Uri activityId, string agent, Guid? registration = null, DateTime? since = null)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            try
            {
                Agent agent2 = Agent.Parse(agent);

                var states = stateResource.GetStates(activityId, agent2, registration, since);

                return Request.CreateResponse(HttpStatusCode.OK, states);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
