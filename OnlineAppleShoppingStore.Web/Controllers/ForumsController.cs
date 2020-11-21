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
    public class ForumsController : Controller
    {
        private readonly IForumsRepository repository;
        private readonly ICommentsRepository repository_c;

        Uri baseAddress = new Uri("https://localhost:44328/api");
        HttpClient client;

        public ForumsController(IForumsRepository objIrepository, 
            ICommentsRepository objIRepository_c)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;

            repository = objIrepository;
            repository_c = objIRepository_c;
        }

        // GET: Forum        
        [Authorize(Roles = "Administrator, User")]
        public ActionResult Index()
        {

            List<Forum> modelList = new List<Forum>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/ForumsApi/GetForums").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<Forum>>(data);
            }

            return View(modelList.ToList());
        }

        // GET: Forum/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id,Description,Title")] Forum forum)
        {

            string data = JsonConvert.SerializeObject(forum);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(client.BaseAddress + "/ForumsApi/AddForum", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Forum/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {

            Forum model = new Forum();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/ForumsApi/GetForumById?id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                string datamodel  = data.Replace("[", string.Empty).Replace("]", string.Empty);
                model = JsonConvert.DeserializeObject<Forum>(datamodel);
            }

            return View(model);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Description,Title")] Forum forum)
        {
            string data = JsonConvert.SerializeObject(forum);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PutAsync(client.BaseAddress + "/ForumsApi/UpdateForum", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View(forum);
        }

        // GET: Forum/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {

            Forum model = new Forum();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/ForumsApi/GetForumById?id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                string datamodel = data.Replace("[", string.Empty).Replace("]", string.Empty);
                model = JsonConvert.DeserializeObject<Forum>(datamodel);
            }

            return View(model);

        }

        // POST: Forum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            HttpResponseMessage response = client.DeleteAsync(client.BaseAddress + "/ForumsApi/DeleteForum?Id=" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, User")]
        public ActionResult CommentsByForum(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Comment> cmt = new List<Comment>();
            var comment = repository_c.All.Where(p => p.ForumId == id).ToList();

            if (comment == null)
            {
                return HttpNotFound();
            }

            foreach (var item in comment)
            {
                cmt.Add(item);
            }

            var forum = repository.All.Where(p => p.Id == id).Select(p => p.Title);

            foreach(var item in forum)
            {
                ViewBag.Forum = item;
            }
                      
            return View(comment);
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
