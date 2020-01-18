using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();
        Statistics s = new Statistics();

        // GET: Statistics
        public ActionResult Index()
        {
            var data = (from orders in db.Orders
                        group orders by orders.DateCreated into dateGroup
                        select new OrderDateList()
                        {
                            Date = dateGroup.Key,
                            Count = dateGroup.Count()
                        }).Take(10);

            var allData = (from orders in db.Orders
                           group orders by orders.DateCreated into dateGroup
                           select new OrderDateList()
                           {
                               Date = dateGroup.Key,
                               Count = dateGroup.Count()
                           });

            s.OrderData = data.ToList();
            s.OrderDataToday = allData.ToList();

            return View(s);
        }

        [HttpGet]
        public JsonResult GetDataAsJson()
        {
            var allData = (from orders in db.Orders
                           group orders by orders.DateCreated into dateGroup
                           select new OrderDateList()
                           {
                               Date = dateGroup.Key,
                               Count = dateGroup.Count()
                           });

            var data = allData.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}