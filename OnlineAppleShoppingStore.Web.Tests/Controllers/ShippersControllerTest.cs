using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Web.Controllers;
using Xunit;

namespace OnlineAppleShoppingStore.Web.Tests.Controllers
{
    public class ShippersControllerTest
    {
        public Mock<IShippersRepository> mock = new Mock<IShippersRepository>();


        [Fact]
        public  void EditShipperById()
        {
            mock.Setup(p => p.Find(1));
            ShippersController s = new ShippersController(mock.Object);
            string result =  s.Edit(1).ToString();
            Assert.Equal("JK", result);
        }
    }
}
