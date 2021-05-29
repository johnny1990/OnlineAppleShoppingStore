using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
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
            mock.Setup(p => p.Find(2));
            ShippersController s = new ShippersController(mock.Object);
            string result =  s.Edit(2).ToString();
            Assert.AreEqual(result, result);
        }

        [TestMethod]
        public void DeleteShipperById()
        {
            var shippersDTO = new ShippersOrder()
            {
                Id = 2,
                Name = "c",
                Phone = "22233"
            };

            mock.Setup(p => p.Find(2));
            ShippersController sc = new ShippersController(mock.Object);
            var result = sc.Delete(2);
            Assert.AreEqual(shippersDTO,result);
        }
    }
}
