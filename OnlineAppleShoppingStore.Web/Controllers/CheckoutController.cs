using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RestSharp;

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

        [Authorize(Roles = "Customer, Administrator")]
        [HttpPost]
        public ActionResult Payment([Bind(Include = "Id,FirstName,LastName,Address,City,State,PostalCode,Country,Phone,Email,DateCreated,Amount,CustomerUserName")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                var cart = ShoppingCart.GetCart(this.HttpContext);
                order = cart.CreateOrder(order);

                SendConfirmationMailOrder(order.FirstName, "Your Order: " + order.Id, order.ToString(), order.Email);

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

        //to be implemented!!!!!!
        private static RestResponse SendConfirmationMailOrder(String toCustomerName, String subject, String body, String destination)
        {         
            return null;
        }
    }
}