using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ShippersController : Controller
    {
        private readonly IShippersRepository repository;

        public ShippersController(IShippersRepository objIrepository)
        {
            repository = objIrepository;
        }


        // GET: ShippersOrders
        public ActionResult Index()
        {
            return View(repository.All.ToList());
        }

        // GET: ShippersOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertShipper(ShippersOrder s)
        {
            if (ModelState.IsValid)
            {
                repository.Insert(s);
                repository.Save();
            }
            return Json(s);
        }

        // GET: ShippersOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippersOrder shippersOrder = repository.Find(id);
            ViewBag.ShipperId = id;
            if (shippersOrder == null)
            {
                return HttpNotFound();
            }
            return View(shippersOrder);
        }


        [HttpPost]
        public JsonResult UpdateShipper(ShippersOrder s)
        {
            if (ModelState.IsValid)
            {
                repository.Update(s);
                repository.Save();
                return Json("Ok");
            }
            else
                return Json("Not ok");
        }

        // GET: ShippersOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippersOrder shippersOrder = repository.Find(id);
            ViewBag.ShipperId = id;
            if (shippersOrder == null)
            {
                return HttpNotFound();
            }
            return View(shippersOrder);
        }

        [HttpPost]
        public JsonResult DeleteShipper(int id)
        {
            repository.Delete(id);
            repository.Save();
            return Json("");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
