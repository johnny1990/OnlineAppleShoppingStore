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
    public class DeliverOrdersController : Controller
    {
        private readonly IDeliverOrdersRepository repository;
        private readonly IOrdersRepository repository_o;
        OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        public DeliverOrdersController(IDeliverOrdersRepository objIrepository, IOrdersRepository objIRepository_o)
        {
            repository = objIrepository;
            repository_o = objIRepository_o;
        }

        // GET: DeliverOrders
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var deliverOrders = repository.Alls.Include(d => d.Order);
                return View(deliverOrders.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        // GET: DeliverOrders/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliverOrder deliverOrder = repository.Find(id);
            if (deliverOrder == null)
            {
                return HttpNotFound();
            }
            return View(deliverOrder);
        }

        // GET: DeliverOrders/Create
        [HttpGet]
        public ActionResult Create()
        {
            var dif = from ord in db.Orders
                     join dor in db.DeliverOrders on ord.Id equals dor.OrderId
                     where ord.Id == dor.OrderId
                     select ord;

            var qry = from ord in db.Orders
                      from dor in db.DeliverOrders
                      where ord.Id != dor.OrderId
                      select ord;

            var orderList = qry.Except(dif);
            ViewBag.OrderId = new SelectList(orderList, "Id", "Id");


            var statusData = from Status e in Enum.GetValues(typeof(Status))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };

            ViewBag.StatusList = new SelectList(statusData, "Name", "Name");

            return View();
        }

        // POST: DeliverOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderId,FirstName,LastName,Address,Phone,Email,OrderDate,Amount,DeliveryDate,Status")] DeliverOrder deliverOrder)
        {
            if (ModelState.IsValid)
            {
                repository.Insert(deliverOrder);
                repository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "FirstName", deliverOrder.OrderId);
            return View(deliverOrder);
        }

        // GET: DeliverOrders/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliverOrder deliverOrder = repository.Find(id);
            if (deliverOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "Id", deliverOrder.OrderId);

            var statusData = from Status e in Enum.GetValues(typeof(Status))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.ToString()
                             };
            ViewBag.StatusList = new SelectList(statusData, "Name", "Name");

            return View(deliverOrder);
        }

        // POST: DeliverOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderId,FirstName,LastName,Address,Phone,Email,OrderDate,Amount,DeliveryDate,Status")] DeliverOrder deliverOrder)
        {
            if (ModelState.IsValid)
            {
                repository.Update(deliverOrder);
                repository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "FirstName", deliverOrder.OrderId);
            return View(deliverOrder);
        }

        [HttpGet]
        public ActionResult GetOrderDetails(string orderId)
        {
            int Id = Convert.ToInt32(orderId);

            var orders = from ord in repository_o.All
                            where ord.Id == Id
                            select ord;

            List<string> ordList = new List<string>();
            foreach (var item in orders)
            {
                ordList.Add(item.FirstName);
                ordList.Add(item.LastName);
                ordList.Add(item.Address);
                ordList.Add(item.Phone);
                ordList.Add(item.Email);
                ordList.Add(item.DateCreated.ToString());
                ordList.Add(item.Amount.ToString());
            }
            return Json(ordList, JsonRequestBehavior.AllowGet);
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
