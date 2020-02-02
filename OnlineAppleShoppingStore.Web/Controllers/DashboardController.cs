using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class DashboardController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();
        // GET: Index??????
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
    }
}