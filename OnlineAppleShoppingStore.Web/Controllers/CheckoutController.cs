using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RestSharp;
using System.Net.Mail;
using System.Net;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        [Authorize(Roles = "Customer, Administrator")]
        [HttpGet]
        public ActionResult Payment()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new OnlineAppleShoppingStore.Web.Utilities.Cart
            {
                CartTotal = cart.GetTotal()
            };

            ViewBag.Amount = viewModel.CartTotal;

            return View();
        }

        [Authorize(Roles = "Customer, Administrator")]
        [HttpPost]
        public ActionResult Payment([Bind(Include = "Id,FirstName,LastName,Address,City,State,PostalCode,Country,Phone,Email,DateCreated,Amount,CustomerUserName")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                var cart = ShoppingCart.GetCart(this.HttpContext);
                order = cart.CreateOrder(order);

                SendConfirmationMailOrder(order.FirstName, "Your Order: " + order.Id, order.ToString(), order.Email);

                return RedirectToAction("Complete", new { id = order.Id });
            }

            return View(order);
        }

        public ActionResult Complete(int id)
        {
            bool isValid = db.Orders.Any(
                o => o.Id == id &&
                     o.CustomerUserName == User.Identity.Name
                );

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

        private static RestResponse SendConfirmationMailOrder(String toCustomerName, String subject, String body, String destination)
        {   
            try
            {
                if(toCustomerName != null && destination != null)
                {
                    var senderEmail = new MailAddress("mail", "Online Apple Shopping Store Order Confirmation");
                    var receiverEmail = new MailAddress(destination, toCustomerName);
                    var password = "password";
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
                    }
                }

                RestClient client = new RestClient();
                RestRequest request = new RestRequest();
                request.AddParameter("from", "Online Apple Shopping Store Order Confirmation" + " <" + "mail" + ">");
                request.AddParameter("to", toCustomerName + " <" + destination + ">");
                request.AddParameter("subject", subject);
                request.AddParameter("html", body);
                request.Method = Method.POST;
                IRestResponse executor = client.Execute(request);
                return executor as RestResponse;
            }
            catch(Exception ex)
            {
                 ex.Message.ToString();
                return null as RestResponse;
            }
        }
    }
}