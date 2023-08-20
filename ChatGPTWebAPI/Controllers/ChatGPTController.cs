using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using ChatGPTWebAPI.SharedObjects;
using ChatGPTWebAPI.Library;
using Queries = ChatGPTWebAPI.SharedObjects.Queries;
using System.Web.Routing;

namespace ChatGPTWebAPI.Controllers
{
    public class ChatGPTController : ApiController
    {
        [AcceptVerbs("Get")]
        [Route("endpoint")]
        //[BasicHttpAuthorize(RequireAuthentication = false)]
        public HttpResponseMessage Get(int userID)
        {
            List<SharedObjects.Endpoint> response = null;

            try
            {
                response = Library.Endpoint.ExportUserEndpoint(userID);
            }
            catch (HttpResponseException ex)
            {
                Utility.ThrowException(ex.Response.ReasonPhrase, Request);
            }
            catch (Exception ex)
            {
                Utility.ThrowSystemException(ex, Request);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [AcceptVerbs("Get")]
        [Route("retrieve")]
        //[BasicHttpAuthorize(RequireAuthentication = false)]
        public HttpResponseMessage Get(DateTime beginDate, DateTime endDate)
        {
            List<Queries> response = null;

            try
            {
                response = Library.Queries.ExportUserQueries(beginDate, endDate, 1);
            }
            catch (HttpResponseException ex)
            {
                Utility.ThrowException(ex.Response.ReasonPhrase, Request);
            }
            catch (Exception ex)
            {
                Utility.ThrowSystemException(ex, Request);
            }

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        [AcceptVerbs("Put")]
        [Route("put")]
        //[BasicHttpAuthorize(RequireAuthentication = true)]
        public HttpResponseMessage Put(List<SharedObjects.Queries> list)
        {
            ImportResponse<SharedObjects.Queries> response = null;

            try
            {
                response = Library.Queries.ImportUserQueries(list);
            }
            catch (HttpResponseException ex)
            {
                Utility.ThrowException(ex.Response.ReasonPhrase, Request);
            }
            catch (Exception ex)
            {
                Utility.ThrowSystemException(ex, Request);
            }

            if (response.InvalidObjects.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, response);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
        }
    }
}
