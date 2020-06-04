using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class CartController : Controller
    {   
        private readonly IProductsRepository repository;
        private readonly ICartsRepository repository_c;

        public CartController(IProductsRepository objIrepository, ICartsRepository objIrepository_c)
        {
            repository = objIrepository;
            repository_c = objIrepository_c;
        }

        public ActionResult Index()
        {
            try
            {
                var cart = ShoppingCart.GetCart(this.HttpContext);

                var viewModel = new OnlineAppleShoppingStore.Web.Utilities.Cart
                {
                    CartItems = cart.GetCartItems(),
                    CartTotal = cart.GetTotal()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        public ActionResult PrintAllReport()
        {    
            var report = new Rotativa.ActionAsPdf("Index");
            return report;
        }

        [ChildActionOnly]
        public ActionResult Summary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("Summary");
        }


        public ActionResult AddToCart(int id)
        {
            var addedProduct = repository.All.Single(product => product.Id == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string productName = repository_c.All.FirstOrDefault(item => item.ProductId == id).Product.Name;

            int itemCount = cart.RemoveFromCart(id);

            var results = new OnlineAppleShoppingStore.Web.Utilities.CartRemove
            {
                Message = Server.HtmlEncode(productName) + " has been removed from your shopping cart",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };

            return Json(results);
        }  
    }
}