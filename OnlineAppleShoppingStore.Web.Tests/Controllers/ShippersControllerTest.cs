using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Web.Controllers;

namespace OnlineAppleShoppingStore.Web.Tests.Controllers
{
    [TestClass]
    public class ShippersControllerTest
    {
        public Mock<IShippersRepository> mock = new Mock<IShippersRepository>();


        [TestMethod]
        public void EditShipperById()
        {
            mock.Setup(p => p.Find(1));
            ShippersController s = new ShippersController(mock.Object);
            string result =  s.Edit(1).ToString();
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("c", result);
        }
    }
}
