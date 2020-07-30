using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using PagedList;
using System.Data;
using ClosedXML.Excel;


namespace OnlineAppleShoppingStore.Web.Controllers
{
    public class ProductsController : Controller
    {
        private OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        private readonly IProductsRepository repository;
        private readonly ICategoryRepository repository_c;

        public ProductsController()
        {

        }

        public ProductsController(IProductsRepository objIrepository, ICategoryRepository objIrepository_c)
        {
            repository = objIrepository;
            repository_c = objIrepository_c;
        }

        // GET: Products
        [Authorize(Roles = "Administrator")]
        public ActionResult Index(int? page)
        {
            try
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
                var products = db.Products.Include(p => p.Category);
                return View(products.ToList().ToPagedList(page ?? 1, 10));
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }         
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public FileResult ExportProductsToExcel()
        {
            OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Name"),
                                            new DataColumn("Price"),
                                            new DataColumn("Description"),
                                            new DataColumn("LastUpdated") });

            var products = from product in db.Products.Include(p => p.Category)
                            select product;

            foreach (var product in products)
            {
                dt.Rows.Add(product.Name, product.Price, product.Description, product.LastUpdated);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductsReport_"+DateTime.Now+".xlsx");
                }
            }
        }

        //FilterProductByCategory functionality
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index(int CategoryId, int? page)
        {   
            if(CategoryId == 0)
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
                var products = db.Products.Include(p => p.Category);
                return View(products.ToList().ToPagedList(page ?? 1, 10));
            }
            else
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
                var products = db.Products.Include(p => p.Category).Where(a => a.CategoryId == CategoryId);
                return View(products.ToList().ToPagedList(page ?? 1, 10));
            }
            
        }

        // GET: Products/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Description,LastUpdated,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Description,LastUpdated,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);

        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Upload()
        {
            DirectoryInfo salesFTPDirectory = null;
            FileInfo[] files = null;

            try
            {
                string salesFTPPath = (Server.MapPath("~/Content/Images/"));
                salesFTPDirectory = new DirectoryInfo(salesFTPPath);
                files = salesFTPDirectory.GetFiles();
            }
            catch (DirectoryNotFoundException exp)
            {
                exp.Message.ToString();
            }
            catch (IOException exp)
            {
                exp.Message.ToString();
            }

            files = files.OrderBy(f => f.Name).ToArray();
            return View(files);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult UploadImage()
        {
            string FileName = "";
            HttpFileCollectionBase files = Request.Files;

            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                string fname;


                var supportedTypes = new[] { "jpg", "png"};
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                {
                    //
                }
                else
                {
                string[] Files = file.FileName.Split(new char[] { '\\' });
                fname = Files[Files.Length - 1];
               
                fname = Path.Combine(Server.MapPath("~/Content/Images/"), fname);
                file.SaveAs(fname);
                }
            }
            ViewBag.FileStatus = "Product image uploaded successfully.";
            return Json(FileName, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}