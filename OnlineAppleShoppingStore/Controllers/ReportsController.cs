using OnlineAppleShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Controllers
{
    public class ReportsController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();


        public ActionResult OrdersReport()
        {
            return View(db.Orders.ToList());

        }

        public ActionResult ProductsOrderedReport()
        {
            return View(db.ProductsOrdereds.ToList());
        }
    }
}