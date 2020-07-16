using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public OrdersRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<Order> All
        {
            get { return _db.Orders; }
        }

        public DbQuery<Order> Alls
        {
            get { return _db.Orders; }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
