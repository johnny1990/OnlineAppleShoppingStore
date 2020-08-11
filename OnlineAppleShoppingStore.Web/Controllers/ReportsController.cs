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
    }
}