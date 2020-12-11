using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineAppleShoppingStore.Web.Controllers.API
{
    public class AdministrationApiController : ApiController
    {
        AuthEntities db = new AuthEntities();

        [HttpGet]
        [Route("api/AdministrationApi/GetUsers")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                var result = from a in db.AspNetUsers.ToList()
                             select new
                             {
                                 a.Id,
                                 a.Email,
                                 a.EmailConfirmed,
                                 a.PasswordHash,
                                 a.SecurityStamp,
                                 a.PhoneNumber,
                                 a.PhoneNumberConfirmed,
                                 a.TwoFactorEnabled,
                                 a.LockoutEndDateUtc,
                                 a.LockoutEnabled,
                                 a.AccessFailedCount,
                                 a.UserName
                             };
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }


    }
}
