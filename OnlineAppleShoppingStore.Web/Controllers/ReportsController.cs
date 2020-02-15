using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IOrdersRepository repository;
        private readonly IProductsOrderedRepository repository_o;

        public ReportsController(IOrdersRepository objIrepository, IProductsOrderedRepository objIrepository_o)
        {
            repository = objIrepository;
            repository_o = objIrepository_o;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult OrdersReport()
        {
            try
            {
                return View(repository.All.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
            

        }

        [Authorize(Roles = "Administrator")]
        public ActionResult ProductsOrderedReport()
        { 
            try
            {
                return View(repository_o.All.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
            
        }
    }
}