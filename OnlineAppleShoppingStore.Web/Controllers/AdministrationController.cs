using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IUsersRepository repository;
        private readonly IRolesRepository repository_R;

        public AdministrationController(IUsersRepository objIrepository, IRolesRepository objIrepository_R)
        {
            repository = objIrepository;
            repository_R = objIrepository_R;
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        #region Users       
        [HttpGet]
        public ActionResult UsersList()
        {
            return View(repository.All.ToList());
        }

  
        public ActionResult EditUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = repository.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edituser([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                repository.Update(aspNetUser);
                repository.Save();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }



        // [Authorize(Roles = "Administrator")]
        public ActionResult DeleteUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser user = repository.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            repository.Save();
            return RedirectToAction("Index");
        }
        #endregion

        #region Roles

        #endregion

    }
}