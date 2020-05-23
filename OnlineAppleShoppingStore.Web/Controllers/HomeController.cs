﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net;
using OnlineAppleShoppingStore.Web.Models;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

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
                            db.Feedbacks.Add(model);
                            db.SaveChanges();                     
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
    }
}