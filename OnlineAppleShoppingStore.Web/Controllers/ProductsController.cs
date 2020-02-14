using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using PagedList;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        private readonly IProductsRepository repository;
        private readonly ICategoryRepository repository_c;

        public ProductsController(IProductsRepository objIrepository, ICategoryRepository objIrepository_c)
        {
            repository = objIrepository;
            repository_c = objIrepository_c;
        }

        // GET: Products
        [Authorize(Roles = "Administrator")]
        public ActionResult Index(int? page)
        {
            var products = db.Products.Include(p => p.Category);

            return View(products.ToList().ToPagedList(page ?? 1, 10));

            //var products = repository.All.Include(p => p.Category);

            // return View(repository.All.ToList().ToPagedList(page ?? 1, 10));
        }

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
          //ViewBag.CategoryId = new SelectList(repository.All, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,LastUpdated,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //if (ModelState.IsValid)
            //{
            //    repository.Insert(product);
            //    repository.Save();
            //    return RedirectToAction("Index");
            //}


             ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);

            //ViewBag.CategoryId = new SelectList(repository.All, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);


        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = repository.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ViewBag.CategoryId = new SelectList(repository.All, "Id", "Name", product.CategoryId);
        //    return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,LastUpdated,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);


            //if (ModelState.IsValid)
            //{
            //    repository.Update(product);
            //    repository.Save();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.CategoryId = new SelectList(repository.All, "Id", "Name", product.CategoryId);
            //return View(product);

        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Product product = repository.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");

            //repository.Delete(id);
            //repository.Save();
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}