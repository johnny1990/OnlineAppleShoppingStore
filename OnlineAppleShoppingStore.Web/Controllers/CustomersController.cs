using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CustomersController : Controller
    {
        private readonly ICustomersRepository repository;
        private readonly IOrdersRepository repository_o;

        public CustomersController(ICustomersRepository objIrepository, IOrdersRepository objIRepository_o)
        {
            repository = objIrepository;
            repository_o = objIRepository_o;
        }

        // GET: Customers
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var customers = repository.Alls.Include(c => c.Order);
                return View(customers.ToList());
            }           
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public FileResult ExportCustomersToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("OrderId"),
                                            new DataColumn("FirstName"),
                                            new DataColumn("LastName"),
                                            new DataColumn("Address"),
                                            new DataColumn("Email")});

            var customers = from customer in repository.All
                         select customer;

            foreach (var cst in customers)
            {
                dt.Rows.Add(cst.OrderId,cst.FirstName, cst.LastName, cst.Address, cst.Email);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomersReport_" + DateTime.Now + ".xlsx");
                }
            }
        }

        // GET: Customers/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "Id");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderId,FirstName,LastName,Address,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                repository.Insert(customer);
                repository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "FirstName", customer.OrderId);
            return View(customer);
        }

        [HttpGet]
        public ActionResult GetCustomersDetails(string orderId)
        {
            int Id = Convert.ToInt32(orderId);
           
            var customers = from cst in repository_o.All
                            where cst.Id == Id
                            select cst;

            List<string> cstList = new List<string>();
            foreach(var item in customers)
            {
                cstList.Add(item.FirstName);
                cstList.Add(item.LastName);
                cstList.Add(item.Address);
                cstList.Add(item.Email);
            }
            return Json(cstList, JsonRequestBehavior.AllowGet);
        }

        // GET: Customers/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = repository.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "Id", customer.OrderId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderId,FirstName,LastName,Address,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                repository.Update(customer);
                repository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "FirstName", customer.OrderId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = repository.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = repository.Find(id);
            repository.Delete(id);
            repository.Save();
            return RedirectToAction("Index");
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
