using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize(Roles = "Administrator, User")]
    public class CommentsController : Controller
    {

        private readonly ICommentsRepository repository;
        private readonly IForumsRepository repository_f;

        Uri baseAddress = new Uri("https://localhost:44328/api");
        HttpClient client;

        public CommentsController(ICommentsRepository objIrepository,
            IForumsRepository objIRepository_f)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;

            repository = objIrepository;
            repository_f = objIRepository_f;
        }


        public ActionResult Index()
        {
            List<Comment> modelList = new List<Comment>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/CommentsApi/GetComments").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<Comment>>(data);
            }

            return View(modelList.ToList());
        }


        public ActionResult Create()
        {
            ViewBag.ForumId = new SelectList(repository_f.All.ToList(), "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ForumId,Body,Title")] Comment comment)
        {
            string data = JsonConvert.SerializeObject(comment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/CommentsApi/AddComment", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ForumId = new SelectList(repository_f.All.ToList(), "Id", "Description", comment.ForumId);
            return View();
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = repository.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ForumId = new SelectList(repository_f.All.ToList(), "Id", "Title", comment.ForumId);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ForumId,Body,Title")] Comment comment)
        {
            string data = JsonConvert.SerializeObject(comment);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/CommentsApi/UpdateComment", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            ViewBag.ForumId = new SelectList(repository_f.All.ToList(), "Id", "Title", comment.ForumId);

            return View(comment);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = repository.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/CommentsApi/DeleteComment?Id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
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
