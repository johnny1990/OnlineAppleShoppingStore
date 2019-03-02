using OnlineAppleShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Controllers
{

    [Authorize]
    public class CheckoutController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();
        const String PromoCode = "FREE";

        [Authorize(Roles = "Customer, Administrator")]
        public ActionResult Payment()
        {
            return View();
        }
        [Authorize(Roles = "Customer, Administrator")]
        [HttpPost]
        public ActionResult Payment(FormCollection values)
        {
            var order = new Order();

            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.CustomerUserName = User.Identity.Name;
                    order.DateCreated = DateTime.Now;

                    db.Orders.Add(order);
                    db.SaveChanges();

                    var cart = ShoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    db.SaveChanges();//we have received the total amount lets update it

                    return RedirectToAction("Complete", new { id = order.Id });
                }
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
                return View(order);
            }
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