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
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
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

        [HttpPost]
        public FileResult ExportProductsToPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;

            string strPDFFileName = string.Format("ProductsReport_" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            Document doc = new Document();
            doc.SetMargins(0f, 0f, 0f, 0f);

            PdfPTable tableLayout = new PdfPTable(6);
            doc.SetMargins(0f, 0f, 0f, 0f); 

            string strAttachment = Server.MapPath("~/Downloadss/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();
 
            doc.Add(Add_Content_To_PDF(tableLayout));

            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);

        }

        //FilterProductByCategory functionality
        [HttpPost]
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
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public JsonResult InsertProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return Json(product);
        }

        // GET: Products/Edit/5
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
            ViewBag.ProductId = id;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public JsonResult UpdateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Ok");
            }
            else
                return Json("Not ok");
        }

        // GET: Products/Delete/5
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

            ViewBag.ProductId = id;
            return View(product);
        }

        [HttpPost]
        public JsonResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return Json("");
        }

        [HttpGet]
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

        #region Methods ExportPdf
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 50, 24, 45, 35, 50, 50 }; 
            tableLayout.SetWidths(headers);  
            tableLayout.WidthPercentage = 100; 
            tableLayout.HeaderRows = 1; 

            List<Product> products = repository.All.ToList<Product>();

            tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER
            });

            AddCellToHeader(tableLayout, "Id");
            AddCellToHeader(tableLayout, "Name");
            AddCellToHeader(tableLayout, "Price");
            AddCellToHeader(tableLayout, "Description");
            AddCellToHeader(tableLayout, "LastUpdated");
            AddCellToHeader(tableLayout, "CategoryId");

            foreach (var emp in products)
            {

                AddCellToBody(tableLayout, emp.Id.ToString());
                AddCellToBody(tableLayout, emp.Name);
                AddCellToBody(tableLayout, emp.Price.ToString());
                AddCellToBody(tableLayout, emp.Description);
                AddCellToBody(tableLayout, emp.LastUpdated.ToString());
                AddCellToBody(tableLayout, emp.CategoryId.ToString());
            }

            return tableLayout;
        }

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }

        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
        #endregion

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