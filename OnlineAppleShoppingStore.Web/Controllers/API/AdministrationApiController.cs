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

        [HttpGet]
        [Route("api/AdministrationApi/GetRoles")]
        public IHttpActionResult GetRoles()
        {
            try
            {
                var result = from a in db.AspNetRoles.ToList()
                             select new
                             {
                                 a.Id,
                                 a.Name
                             };
                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("api/AdministrationApi/GetUserRoles")]
        public IHttpActionResult GetUserRoles()
        {
            try
            {
                var result = from a in db.AspNetUserRoles.ToList()
                             select new
                             {
                                 a.Id,
                                 a.UserId,
                                 a.RoleId
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
