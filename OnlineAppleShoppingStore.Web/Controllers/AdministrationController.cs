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
    [Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IUsersRepository repository;
        private readonly IRolesRepository repository_R;
        private readonly IUserRolesRepository repository_U;

        public AdministrationController(IUsersRepository objIrepository, IRolesRepository objIrepository_R,
            IUserRolesRepository objIRepository_U)
        {
            repository = objIrepository;
            repository_R = objIrepository_R;
            repository_U = objIRepository_U;
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Map()
        {
            string mark = "[";
            mark += "{";
            mark += string.Format("'title': '{0}',", "");
            mark += string.Format("'lat': '{0}',", "");
            mark += string.Format("'lng': '{0}',", "");
            mark += string.Format("'description': '{0}'", "");
            mark += "}";
                
            mark += "];";
            ViewBag.Mark = mark;
            return View();
        }

        #region Users       
        [HttpGet]
        public ActionResult Users()
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
        public ActionResult EditUser([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                repository.Update(aspNetUser);
                repository.Save();
                return RedirectToAction("Users");
            }
            return View(aspNetUser);
        }


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

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            repository.Save();
            return RedirectToAction("Users");
        }
        #endregion

        #region Roles
      
        public ActionResult Roles()
        {
            return View(repository_R.All.ToList());
        }

    
        public ActionResult NewRole()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewRole([Bind(Include = "Id,Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                repository_R.Insert(aspNetRole);
                repository_R.Save();
                return RedirectToAction("Roles");
            }

            return View(aspNetRole);
        }

        
        public ActionResult EditRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = repository_R.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser([Bind(Include = "Id,Name")] AspNetRole aspNetRole)
        {
            if (ModelState.IsValid)
            {
                repository_R.Update(aspNetRole);
                repository_R.Save();
                return RedirectToAction("Roles");
            }
            return View(aspNetRole);
        }

        
        public ActionResult DeleteRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = repository_R.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetRole);
        }

        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed2(int id)
        {
            repository.Delete(id);
            repository.Save();
            return RedirectToAction("Roles");
        }
        #endregion

        #region UsersRoles
        [HttpGet]
        public ActionResult UserRoles()
        {
            return View(repository_U.All.ToList());
        }

        [HttpGet]
        public ActionResult NewUserRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUserRole([Bind(Include = "UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {
            if (ModelState.IsValid)
            {
                repository_U.Insert(aspNetUserRole);
                repository_U.Save();
                return RedirectToAction("UserRoles");
            }
            return View(aspNetUserRole);
       }

        [HttpGet]
        public ActionResult EditUserRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = repository_U.Find(id);
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(repository_U.All, "Id", "RoleId", aspNetUserRole.RoleId);
            ViewBag.UserId = new SelectList(repository_U.All, "Id", "UserId", aspNetUserRole.UserId);
            return View(aspNetUserRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRole([Bind(Include = "Id,UserId,RoleId")] AspNetUserRole aspNetUserRole)
        {          
            if (ModelState.IsValid)
            {
                repository_U.Update(aspNetUserRole);
                repository_U.Save();
                return RedirectToAction("userRoles");
            }
            return View(aspNetUserRole);
        }

        public ActionResult DeleteUserRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserRole aspNetUserRole = repository_U.Find(id);
            if (aspNetUserRole == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserRole);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserRoleConfirmed(int id)
        {
            repository_U.Delete(id);
            repository_U.Save();
            return RedirectToAction("UserRoles");
        }       
        #endregion 
    }
}