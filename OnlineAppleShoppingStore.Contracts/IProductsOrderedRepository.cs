using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IProductsOrderedRepository : IDisposable
    {
        IQueryable<ProductsOrdered> All { get; }
    }
}
