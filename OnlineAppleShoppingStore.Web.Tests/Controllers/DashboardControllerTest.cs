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
    public class DashboardControllerTest
    {
        [TestMethod]
        public void Index()
        {
            DashboardController controller = new DashboardController();
            ViewResult result = controller.Index(1) as ViewResult;
            Assert.AreEqual(result, result);
        }


        [TestMethod]
        public void Supplier()
        {
            DashboardController controller = new DashboardController();
            ViewResult result = controller.Supplier() as ViewResult;
            Assert.AreEqual(result, result);
        }

    }
}
