using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class ReportsController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        [Authorize(Roles = "Administrator")]
        public ActionResult OrdersReport()
        {
            return View(db.Orders.ToList());

        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ProductsOrderedReport()
        {
            return View(db.ProductsOrdereds.ToList());
        }
    }
}