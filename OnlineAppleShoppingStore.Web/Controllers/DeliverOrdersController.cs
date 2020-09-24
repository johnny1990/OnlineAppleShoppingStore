using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using Font = iTextSharp.text.Font;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DeliverOrdersController : Controller
    {
        private readonly IDeliverOrdersRepository repository;
        private readonly IOrdersRepository repository_o;
        OnlineAppleShoppingStoreEntities db = new OnlineAppleShoppingStoreEntities();

        public DeliverOrdersController(IDeliverOrdersRepository objIrepository, IOrdersRepository objIRepository_o)
        {
            repository = objIrepository;
            repository_o = objIRepository_o;
        }

        // GET: DeliverOrders
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                var deliverOrders = repository.Alls.Include(d => d.Order);
                return View(deliverOrders.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public FileResult ExportOrdersToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[5] { new DataColumn("FirstName"),
                                            new DataColumn("Address"),
                                            new DataColumn("Amount"),
                                            new DataColumn("DeliveryDate"),
                                            new DataColumn("Status")});

            var orders = from order in repository.All
                            select order;

            foreach (var order in orders)
            {
                dt.Rows.Add(order.FirstName, order.Address, order.Amount, order.DeliveryDate, order.Status);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrdersDeliveredReport_" + DateTime.Now + ".xlsx");
                }
            }
        }

        [HttpPost]
        public FileResult ExportDeliverOrdersToPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;

            string strPDFFileName = string.Format("DeliverOrdersReport_" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
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

        // GET: DeliverOrders/Details/5
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliverOrder deliverOrder = repository.Find(id);
            if (deliverOrder == null)
            {
                return HttpNotFound();
            }
            return View(deliverOrder);
        }

        // GET: DeliverOrders/Create
        [HttpGet]
        public ActionResult Create()
        {
            var dif = from ord in db.Orders
                     join dor in db.DeliverOrders on ord.Id equals dor.OrderId
                     where ord.Id == dor.OrderId
                     select ord;

            var qry = from ord in db.Orders
                      from dor in db.DeliverOrders
                      where ord.Id != dor.OrderId
                      select ord;

            var orderList = qry.Except(dif);
            ViewBag.OrderId = new SelectList(orderList, "Id", "Id");


            var statusData = from Status e in Enum.GetValues(typeof(Status))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };

            var deliverVia = from DeliverVia e in Enum.GetValues(typeof(DeliverVia))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.ToString()
                             };

            ViewBag.StatusList = new SelectList(statusData, "Name", "Name");
            ViewBag.DeliverViaList = new SelectList(deliverVia, "Name", "Name");

            return View();
        }

        // POST: DeliverOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderId,FirstName,LastName,Address,Phone,Email,OrderDate,Amount,DeliveryDate,Status,DeliverVia")] DeliverOrder deliverOrder)
        {
            if (ModelState.IsValid)
            {
                repository.Insert(deliverOrder);
                repository.Save();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "FirstName", deliverOrder.OrderId);
            return View(deliverOrder);
        }

        // GET: DeliverOrders/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliverOrder deliverOrder = repository.Find(id);
            if (deliverOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "Id", deliverOrder.OrderId);

            var statusData = from Status e in Enum.GetValues(typeof(Status))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.ToString()
                             };

            var deliverVia = from DeliverVia e in Enum.GetValues(typeof(DeliverVia))
                             select new
                             {
                                 ID = (int)e,
                                 Name = e.ToString()
                             };

            ViewBag.StatusList = new SelectList(statusData, "Name", "Name");
            ViewBag.DeliverViaList = new SelectList(deliverVia, "Name", "Name");

            return View(deliverOrder);
        }

        // POST: DeliverOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderId,FirstName,LastName,Address,Phone,Email,OrderDate,Amount,DeliveryDate,Status,DeliverVia")] DeliverOrder deliverOrder)
        {
            if (ModelState.IsValid)
            {
                repository.Update(deliverOrder);
                repository.Save();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(repository_o.All, "Id", "FirstName", deliverOrder.OrderId);
            return View(deliverOrder);
        }

        [HttpGet]
        public ActionResult GetOrderDetails(string orderId)
        {
            int Id = Convert.ToInt32(orderId);

            var orders = from ord in repository_o.All
                            where ord.Id == Id
                            select ord;

            List<string> ordList = new List<string>();
            foreach (var item in orders)
            {
                ordList.Add(item.FirstName);
                ordList.Add(item.LastName);
                ordList.Add(item.Address);
                ordList.Add(item.Phone);
                ordList.Add(item.Email);
                ordList.Add(item.DateCreated.ToString());
                ordList.Add(item.Amount.ToString());
            }
            return Json(ordList, JsonRequestBehavior.AllowGet);
        }

        #region Methods ExportPdf to change!!!!!!!
        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 50, 24, 45, 35, 50, 50 };
            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 100;
            tableLayout.HeaderRows = 1;

            List<DeliverOrder> deliverOrders = repository.All.ToList<DeliverOrder>();

            tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            AddCellToHeader(tableLayout, "Id");
            AddCellToHeader(tableLayout, "OrderId");
            AddCellToHeader(tableLayout, "FirstName");
            AddCellToHeader(tableLayout, "LastName");
            AddCellToHeader(tableLayout, "Address");
            AddCellToHeader(tableLayout, "Email");
            AddCellToHeader(tableLayout, "Amount");
            AddCellToHeader(tableLayout, "DeliveryDate");
            AddCellToHeader(tableLayout, "Status");
            AddCellToHeader(tableLayout, "DeliverVia");

            foreach (var dor in deliverOrders)
            {
                AddCellToBody(tableLayout, dor.Id.ToString());
                AddCellToBody(tableLayout, dor.OrderId.ToString());
                AddCellToBody(tableLayout, dor.FirstName);
                AddCellToBody(tableLayout, dor.LastName);
                AddCellToBody(tableLayout, dor.Address);
                AddCellToBody(tableLayout, dor.Email);
                AddCellToBody(tableLayout, dor.Amount.ToString());
                AddCellToBody(tableLayout, dor.DeliveryDate.ToString());
                AddCellToBody(tableLayout, dor.Status);
                AddCellToBody(tableLayout, dor.DeliverVia);                         
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
                repository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
