using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http;
using System.Net;
using CSE545.Group8.Bank.DataAccessLayer;
using CSE545.Group8.Bank.Entities;

namespace CSE545.Group8.Bank.WebAPI.Controllers
{
    public class SystemLogsController : ApiController
    {
        // Get Single Transaction by Transaction ID
        [HttpGet]
        [ResponseType(typeof(List<Entities.Systemlog>))]
        [Authorize(Roles = "Administrator")]
        [Route("api/systemlogs")]
        public HttpResponseMessage getSystemLogs()
        {
            try
            {
                SystemLayer logs = new SystemLayer();
                var listlogs = logs.getSystemLogs();
                if (listlogs == null) // this wont work is T is not a nullable type
                {
                    var message = "No Logs to Display";
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
                return Request.CreateResponse(HttpStatusCode.OK, listlogs);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }

}