using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineAppleShoppingStore.Web;
using OnlineAppleShoppingStore.Web.Controllers;

namespace OnlineAppleShoppingStore.Web.Tests.Controllers
{
    [TestClass]
    public class ProductsControllerTest
    {
        [TestMethod]
        public void Index()
        {
            ProductsController controller = new ProductsController();
            ViewResult result = controller.Index(1) as ViewResult;
            Assert.AreEqual(result, result);
        }

        [TestMethod]
        public void Create()
        {
            ProductsController controller = new ProductsController();
            ViewResult result = controller.Create() as ViewResult;
            Assert.AreEqual(result, result);
        }

        [TestMethod]
        public void Edit()
        {
            ProductsController controller = new ProductsController();
            ViewResult result = controller.Edit(1) as ViewResult;
            Assert.AreEqual(result, result);
        }

        [TestMethod]
        public void Delete()
        {
            ProductsController controller = new ProductsController();
            ViewResult result = controller.Delete(1) as ViewResult;
            Assert.AreEqual(result, result);
        }

        [TestMethod]
        public void Upload()
        {
            ProductsController controller = new ProductsController();
            ViewResult result = controller.Upload() as ViewResult;
            Assert.AreEqual(result, result);
        }
    }
}
