using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;
using OnlineAppleShoppingStore.Web.Models;
using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Contracts;
using Microsoft.AspNet.Identity;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IOrdersRepository repository_o;
        private readonly ICartsRepository repository_c;
        private readonly IProductsRepository repository_p;
        private readonly IFeedbackRepository repository_f;

        public HomeController()
        {

        }

        public HomeController(IFeedbackRepository objIrepository_f, IOrdersRepository objIrepository_o,
             ICartsRepository objIrepository_c, IProductsRepository objIrepository_p)
        {
            repository_f = objIrepository_f;
            repository_o = objIrepository_o;
            repository_c = objIrepository_c;
            repository_p = objIrepository_p;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback([Bind(Include = "Id,FromName,FromEmail,FeedBack1")] Feedback model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("mail", "Online Apple Shopping Store Feedback");
                    var receiverEmail = new MailAddress(model.FromEmail, model.FromName);
                    var password = "password";
                    var subject = "Feedback notification";
                    var body = model.FeedBack1;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var message = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                            smtp.Send(message);                       
                            repository_f.Insert(model);
                            repository_f.Save();                     
                    }
                    return RedirectToAction("Sent");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message.ToString();
            }
            return RedirectToAction("Sent");
        }       

        public ActionResult Sent()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult FeedbackList()
        {
            return View(repository_f.All.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult GeneralStatistics()
        {   
            var totalOrders = repository_o.All.Select(p => p.Amount);
            ViewBag.SumOrders = totalOrders.Sum();

            var nrOrders = repository_o.All.Select(p => p.Id);
            ViewBag.NrOrders = nrOrders.Count();

            var nrProducts = repository_c.All.Select(p => p.Product);
            ViewBag.NrProducts = nrProducts.Count();

            var totalQuantity = repository_c.All.Select(p => p.Count);
            ViewBag.TotalQuantity = totalQuantity.Sum();

            var productsAvailables = repository_p.All.Select(p => p.Id);
            ViewBag.ProductsSales = productsAvailables.Count();

            var reviews = repository_f.All.Select(p => p.Id);
            ViewBag.Reviews = reviews.Count();

            return View();
        }
            
    }
}