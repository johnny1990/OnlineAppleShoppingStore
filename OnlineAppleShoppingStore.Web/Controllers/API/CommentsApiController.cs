using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineAppleShoppingStore.Web.Controllers.API
{
    public class CommentsApiController : ApiController
    {
        OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        [HttpGet]
        [Route("api/CommentsApi/GetComments")]
        public HttpResponseMessage GetComments()
        {
            try
            {
                var result = from a in db.Comments.ToList()
                             select new
                             {   a.Id,
                                 a.ForumId, 
                                 a.Body,
                                 a.Title,
                                 a.Forum.Description
                             };
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error!");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCommentsById(int id)
        {
            try
            {
                var result = from a in db.Comments.Where( p => p.Id == id)
                             select new
                             {
                                 a.Id,
                                 a.ForumId,
                                 a.Body,
                                 a.Title,
                                 a.Forum.Description
                             };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error!");
            }
        }

        [HttpPost]
        public HttpResponseMessage AddComment([FromBody] Comment model)
        {
            try
            {
                db.Comments.Add(model);
                db.SaveChanges();
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Created);
                return response;
            }
            catch
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateComment([FromBody] Comment model)
        {
            try
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
            catch
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                return response;
            }
        }

        [HttpDelete]
        public HttpResponseMessage DeleteComment(int id)
        {
            Comment comm = db.Comments.Find(id);

            if (comm != null)
            {
                db.Comments.Remove(comm);
                db.SaveChanges();
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                return response;
            }
            else
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return response;
            }
        }

    }
}
