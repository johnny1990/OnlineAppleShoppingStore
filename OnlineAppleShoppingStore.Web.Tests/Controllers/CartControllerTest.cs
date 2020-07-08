using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineAppleShoppingStore.Web.Controllers;

namespace OnlineAppleShoppingStore.Web.Tests.Controllers
{
    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void Index()
        {
            CartController controller = new CartController();
            ViewResult result = controller.Index() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Summary()
        {
            CartController controller = new CartController();
            ViewResult result = controller.Summary() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddToCart(int id)
        {
            CartController controller = new CartController();
            ViewResult result = controller.AddToCart(id) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RemoveFromCart(int id)
        {
            CartController controller = new CartController();
            ViewResult result = controller.RemoveFromCart(id) as ViewResult;
            Assert.IsNotNull(result);
        }

    }
}
