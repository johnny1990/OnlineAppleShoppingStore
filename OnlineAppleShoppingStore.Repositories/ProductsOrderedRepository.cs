using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class ProductsOrderedRepository : IProductsOrderedRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public ProductsOrderedRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<ProductsOrdered> All
        {
            get { return _db.ProductsOrdereds; }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
