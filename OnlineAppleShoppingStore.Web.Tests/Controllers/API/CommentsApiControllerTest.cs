using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineAppleShoppingStore.Web.Controllers.API;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OnlineAppleShoppingStore.Web.Tests.Controllers.API
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CommentsApiControllerTest
    {

        [TestMethod]
        public void TestGetComments()
        {
            CommentsApiController ct = new CommentsApiController();
            HttpResponseMessage result = ct.GetComments();
            Assert.IsNotNull(result);
        }
    }
}
