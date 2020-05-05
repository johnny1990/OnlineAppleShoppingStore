using OnlineAppleShoppingStore.Contracts;
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
        private readonly ICategoryRepository repository;
        private readonly IProductsRepository repository_p;

        public DashboardController(ICategoryRepository objIrepository, IProductsRepository objIrepositoryP)
        {
            repository = objIrepository;
            repository_p = objIrepositoryP;
        }

        // GET: Index
        public ActionResult Index(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Category category = repository.Find(id);
                if (category == null)
                {
                    return HttpNotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Browse(string Category)
        {
            var category = repository.Alls.Include("Products")
               .Single(g => g.Name == Category);

            return View(category);
        }

        [ChildActionOnly]
        [HttpGet]
        public ActionResult Menu()
        {
            var catagories = repository.All.ToList();
            return PartialView(catagories);
        }

        [HttpGet]
        public ActionResult ProductDetails(int id)
        {
            var item = repository_p.Find(id);
            return View(item);
        }
    }
}