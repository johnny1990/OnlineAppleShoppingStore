using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineAppleShoppingStore.Web.Controllers.API
{
    public class FeedbackApiController : ApiController
    {
        OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        [HttpGet]
        public HttpResponseMessage GetFeedbacks()
        {
            var result = db.Feedbacks.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

    }
}
