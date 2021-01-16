using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class ShippersRepository : IShippersRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public ShippersRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<ShippersOrder> All
        {
            get { return _db.ShippersOrders; }
        }

        public void Delete(int id)
        {
            var sh = _db.ShippersOrders.Find(id);
            _db.ShippersOrders.Remove(sh);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public ShippersOrder Find(int? id)
        {
            ShippersOrder objS = new ShippersOrder();
            objS = _db.ShippersOrders.Where(p => p.Id == id).FirstOrDefault();
            return objS;
        }

        public void Insert(ShippersOrder so)
        {
            _db.ShippersOrders.Add(so);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(ShippersOrder so)
        {
            _db.Entry(so).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
