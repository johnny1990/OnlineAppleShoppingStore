using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineAppleShoppingStore.Web.Models
{
    public class OrderDateList
    {
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public int Count { get; set; }
    }
}