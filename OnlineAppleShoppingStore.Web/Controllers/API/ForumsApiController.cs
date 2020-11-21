using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineAppleShoppingStore.Web.Controllers.API
{
    public class ForumsApiController : ApiController
    {
        OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        [HttpGet]
        [Route("api/ForumsApi/GetForums")]
        public HttpResponseMessage GetForums()
        {
            try
            {
                var result = from a in db.Fora.ToList()
                             select new
                             {
                                 a.Id,
                                 a.Description,
                                 a.Title
                             };
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error!");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetForumById(int id)
        {
            try
            {
                var result = from a in db.Fora.Where(p => p.Id == id).ToList()
                             select new
                             {
                                 a.Id,
                                 a.Description,
                                 a.Title
                             };

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Error!");
            }
        }


        [HttpPost]
        public IHttpActionResult AddForum([FromBody] Forum model)
        {
            try
            {
                db.Fora.Add(model);
                db.SaveChanges();
                return Created("Created", model);

            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateForum([FromBody] Forum model)
        {
            try
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteForum(int id)
        {
            Forum f = db.Fora.Find(id);

            if (f != null)
            {
                db.Fora.Remove(f);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
