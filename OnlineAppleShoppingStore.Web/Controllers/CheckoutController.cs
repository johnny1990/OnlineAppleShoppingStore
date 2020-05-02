using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        [Authorize(Roles = "Customer, Administrator")]
        public ActionResult Payment()
        {
            return View();
        }

        //save ordered products!
        [Authorize(Roles = "Customer, Administrator")]
        [HttpPost]
        public ActionResult Payment([Bind(Include = "Id,FirstName,LastName,Address,City,State,PostalCode,Country,Phone,Email,DateCreated,Amount,CustomerUserName")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Complete", new { id = order.Id });
            }

            return View(order);
        }

        public ActionResult Complete(int id)
        {
            bool isValid = db.Orders.Any(
                o => o.Id == id &&
                     o.CustomerUserName == User.Identity.Name
                );

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}