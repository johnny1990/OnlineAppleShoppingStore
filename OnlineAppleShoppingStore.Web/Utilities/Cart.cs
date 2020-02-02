using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAppleShoppingStore.Web.Utilities
{
    public class Cart
    {
        public List<OnlineAppleShoppingStore.Entities.Models.Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}