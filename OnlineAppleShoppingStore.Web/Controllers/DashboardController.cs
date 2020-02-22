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

        public DashboardController(ICategoryRepository objIrepository)
        {
            repository = objIrepository;
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
            catch(Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }                        
        }

        public ActionResult Browse(string cat)
        {
            var categorie = repository.Alls.Include("Products")
               .Single(g => g.Name == cat);

            return View(categorie);
        }

        [ChildActionOnly]
        [HttpGet]
        public ActionResult Menu()
        {
            var catagories = repository.All.ToList();

            return PartialView(catagories);
        }
        //Browse???????
        //etc....
    }
}