using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository repository;

        public CategoriesController(ICategoryRepository objIrepository)
        {
            repository = objIrepository;
        }

        // GET: Categories
        public ActionResult Index()
        {
            try
            {
                return View(repository.All.ToList());
            }
            catch(Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
            
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public JsonResult InsertCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                repository.Insert(category);
                repository.Save();
            }
            return Json(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = repository.Find(id);
            ViewBag.CategoryId = id;
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);

        }

        [HttpPost]
        public JsonResult UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                repository.Update(category);
                repository.Save();
                return Json("Ok");
            }
            else
                return Json("Not ok");
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = repository.Find(id);
            ViewBag.CategoryId = id;
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    repository.Delete(id);
        //    repository.Save();
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public JsonResult DeleteCategory(int id)
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