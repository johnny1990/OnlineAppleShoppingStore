using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class CartsRepository : ICartsRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public CartsRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<Cart> All
        {
            get { return _db.Carts; }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
