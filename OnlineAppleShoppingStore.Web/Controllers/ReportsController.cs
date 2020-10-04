using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using OnlineAppleShoppingStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace OnlineAppleShoppingStore.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ReportsController : Controller
    {
        private readonly IOrdersRepository repository;
        private readonly IProductsOrderedRepository repository_o;
        private readonly ICartsRepository repository_c;

        public ReportsController(IOrdersRepository objIrepository, IProductsOrderedRepository objIrepository_o,
            ICartsRepository objIrepository_c)
        {
            repository = objIrepository;
            repository_o = objIrepository_o;
            repository_c = objIrepository_c;
        }

        public ActionResult OrdersReport(int? page)
        {
            try
            {
                return View(repository.All.ToList().ToPagedList(page ?? 1, 10));

            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        public ActionResult ProductsOrderedReport()
        { 
            try
            {
                return View(repository_o.All.ToList());
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }           
        }

        public ActionResult CartProductsReport(int? page)
        {
            try
            {
                return View(repository_c.All.ToList().ToPagedList(page ?? 1, 10));
            }
            catch (Exception ex)
            {
                Logger.LogWriter.LogException(ex);
                return HttpNotFound();
            }
        }

        #region ExportTOExcel
        [HttpPost]
        public FileResult ExportOrdersToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[13] { new DataColumn("Id"),
                                            new DataColumn("FirstName"),
                                            new DataColumn("LastName"),
                                            new DataColumn("Address"),
                                            new DataColumn("City"),
                                            new DataColumn("State"),
                                            new DataColumn("PostalCode"),
                                            new DataColumn("Countrye"),
                                            new DataColumn("Phone"),
                                            new DataColumn("Email"),
                                            new DataColumn("DateCreated"),
                                            new DataColumn("Amount"),
                                            new DataColumn("CustomerUserName")});

            var orders = from order in repository.All
                            select order;

            foreach (var ord in orders)
            {
                dt.Rows.Add(ord.Id, ord.FirstName, ord.LastName, ord.Address, ord.City, ord.State, ord.PostalCode, ord.Country, ord.Phone, ord.Email, ord.DateCreated, ord.Amount, ord.CustomerUserName);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "OrdersReport_" + DateTime.Now + ".xlsx");
                }
            }
        }

        [HttpPost]
        public FileResult ExportProductsOrderedToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("OrderId"),
                                            new DataColumn("ProductId"),
                                            new DataColumn("Quantity")});

            var po = from order in repository_o.All
                         select order;

            foreach (var ord in po)
            {
                dt.Rows.Add(ord.OrderId, ord.ProductId, ord.Quantity);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductsOrderedReport_" + DateTime.Now + ".xlsx");
                }
            }
        }

        [HttpPost]
        public FileResult ExportProductsInCartToExcel()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("CartId"),
                                            new DataColumn("ProductId"),
                                            new DataColumn("Count"),
                                            new DataColumn("DateCreated")});

            var pr = from cart in repository_c.All
                     select cart;

            foreach (var cart in pr)
            {
                dt.Rows.Add(cart.CartId, cart.ProductId ,cart.Count, cart.DateCreated);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ProductsInCartReport_" + DateTime.Now + ".xlsx");
                }
            }
        }
        #endregion

        #region ExportOrdersToPdf
        [HttpPost]
        public FileResult ExportOrdersToPdf()
        {
            MemoryStream workStream = new MemoryStream();
            StringBuilder status = new StringBuilder("");
            DateTime dTime = DateTime.Now;

            string strPDFFileName = string.Format("OrdersReport_" + dTime.ToString("yyyyMMdd") + "-" + ".pdf");
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0f, 0f, 0f, 0f);

            PdfPTable tableLayout = new PdfPTable(6);
            doc.SetMargins(0f, 0f, 0f, 0f);

            string strAttachment = Server.MapPath("~/Downloads/" + strPDFFileName);


            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            doc.Add(Add_Content_To_PDF(tableLayout));

            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;


            return File(workStream, "application/pdf", strPDFFileName);
        }

        protected PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)
        {

            float[] headers = { 50, 24, 45, 35, 50, 50 };
            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 100;
            tableLayout.HeaderRows = 1;

            List<Order> deliverOrders = repository.All.ToList<Order>();

            tableLayout.AddCell(new PdfPCell(new Phrase("Creating Pdf using ItextSharp", new Font(Font.FontFamily.HELVETICA, 8, 1, new iTextSharp.text.BaseColor(0, 0, 0))))
            {
                Colspan = 12,
                Border = 0,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            AddCellToHeader(tableLayout, "Id");
            AddCellToHeader(tableLayout, "FirstName");
            AddCellToHeader(tableLayout, "LastName");
            AddCellToHeader(tableLayout, "Address");
            AddCellToHeader(tableLayout, "City");
            AddCellToHeader(tableLayout, "Email");
            AddCellToHeader(tableLayout, "Phone");
            AddCellToHeader(tableLayout, "DateCreated");

            foreach (var dor in deliverOrders)
            {
                AddCellToBody(tableLayout, dor.Id.ToString());
                AddCellToBody(tableLayout, dor.FirstName);
                AddCellToBody(tableLayout, dor.LastName);
                AddCellToBody(tableLayout, dor.Address);
                AddCellToBody(tableLayout, dor.City);
                AddCellToBody(tableLayout, dor.Email);
                AddCellToBody(tableLayout, dor.Phone);
                AddCellToBody(tableLayout, dor.Amount.ToString());
                AddCellToBody(tableLayout, dor.DateCreated.ToString());
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
    }
}