using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class ForumsController : Controller
    {
        private readonly IForumsRepository repository;
        private readonly ICommentsRepository repository_c;

        public ForumsController(IForumsRepository objIrepository, 
            ICommentsRepository objIRepository_c)
        {
            repository = objIrepository;
            repository_c = objIRepository_c;
        }

        // GET: Forum        
        [Authorize(Roles = "Administrator, User")]
        public ActionResult Index()
        {
            return View(repository.All.ToList());
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
            if (ModelState.IsValid)
            {
                repository.Insert(forum);
                repository.Save();
                return RedirectToAction("Index");
            }

            return View(forum);
        }

        // GET: Forum/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = repository.Find(id);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }

        // POST: Forum/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Description,Title")] Forum forum)
        {
            if (ModelState.IsValid)
            {
                repository.Update(forum);
                repository.Save();
                return RedirectToAction("Index");
            }
            return View(forum);
        }

        // GET: Forum/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forum forum = repository.Find(id);
            if (forum == null)
            {
                return HttpNotFound();
            }
            return View(forum);
        }

        // POST: Forum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            repository.Delete(id);
            repository.Save();
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
