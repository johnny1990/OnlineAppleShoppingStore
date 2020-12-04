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
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrdersRepository repository_o;
        private readonly ICartsRepository repository_c;
        private readonly IProductsRepository repository_p;
        private readonly IFeedbackRepository repository_f;
        private readonly IDeliverOrdersRepository repository_d;
        private readonly IProductsOrderedRepository repository_po;

        Uri baseAddress = new Uri("https://localhost:44328/api");
        HttpClient client;

        public HomeController()
        {

        }

        public HomeController(IFeedbackRepository objIrepository_f, IOrdersRepository objIrepository_o,
             ICartsRepository objIrepository_c, IProductsRepository objIrepository_p, IDeliverOrdersRepository objIrepository_d,
             IProductsOrderedRepository objIRepository_po)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;

            repository_f = objIrepository_f;
            repository_o = objIrepository_o;
            repository_c = objIrepository_c;
            repository_p = objIrepository_p;
            repository_d = objIrepository_d;
            repository_po = objIRepository_po;
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

        public ActionResult Social()
        {
            return PartialView();
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

                        //

                        string data = JsonConvert.SerializeObject(model);
                        StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/FeedbackApi/InsertFeedback", content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                        //return View();
                        //
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
            try
            {
                List<Feedback> modelList = new List<Feedback>();
                HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/FeedbackApi/GetFeedbacks").Result;
                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    modelList = JsonConvert.DeserializeObject<List<Feedback>>(data);
                }

                return View(modelList.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult GeneralStatistics()
        {
            try
            {
                var nrDelivers = repository_d.All.Where(p => p.Status == "Ordered").Select(p => p.Id);
                ViewBag.Delivers = nrDelivers.Count();

                var totalOrders = repository_o.All.Select(p => p.Amount);
                ViewBag.SumOrders = totalOrders.Sum();

                var nrOrders = repository_o.All.Select(p => p.Id);
                ViewBag.NrOrders = nrOrders.Count();

                var nrProducts = repository_po.All.Select(p => p.Quantity);
                ViewBag.NrProducts = nrProducts.Count();

                var totalQuantity = repository_po.All.Select(p => p.Quantity);
                ViewBag.TotalQuantity = totalQuantity.Sum();

                var productsAvailables = repository_p.All.Select(p => p.Id);
                ViewBag.ProductsSales = productsAvailables.Count();

                var reviews = repository_f.All.Select(p => p.Id);
                ViewBag.Reviews = reviews.Count();

                return View();
            }           
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }         
    }
}