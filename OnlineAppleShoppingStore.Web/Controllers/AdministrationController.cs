using OnlineAppleShoppingStore.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    //[Authorize(Roles = "Administrator")]
    public class AdministrationController : Controller
    {
        private readonly IUsersRepository repository;

        public AdministrationController(IUsersRepository objIrepository)
        {
            repository = objIrepository;
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UsersList()
        {
            return View(repository.All.ToList());
        }

       


    }
}