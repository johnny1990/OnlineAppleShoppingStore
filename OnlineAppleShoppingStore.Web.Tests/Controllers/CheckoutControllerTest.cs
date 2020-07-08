using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineAppleShoppingStore.Web.Controllers;

namespace OnlineAppleShoppingStore.Web.Tests.Controllers
{
    [TestClass]
    public class CheckoutControllerTest
    {
        [TestMethod]
        public void Payment()
        {
            CheckoutController controller = new CheckoutController();
            ViewResult result = controller.Payment() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Complete(int id)
        {
            CheckoutController controller = new CheckoutController();
            ViewResult result = controller.Complete(id) as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
