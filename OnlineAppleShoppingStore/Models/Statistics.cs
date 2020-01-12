using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineAppleShoppingStore.Models
{
    public class Statistics
    {
        public List<OrderDateList> OrderData { get; set; }

        public List<OrderDateList> OrderDataToday { get; set; }
    }
}