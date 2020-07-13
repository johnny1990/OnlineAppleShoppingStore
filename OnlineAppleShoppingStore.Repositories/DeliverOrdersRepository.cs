using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Repositories
{
    public class DeliverOrdersRepository : IDeliverOrdersRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public DeliverOrdersRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<DeliverOrder> All
        {
            get { return _db.DeliverOrders; }
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public DeliverOrder Find(int? id)
        {
            DeliverOrder objOrd = new DeliverOrder();
            objOrd = _db.DeliverOrders.Where(p => p.Id == id).FirstOrDefault();
            return objOrd;
        }

        public void Insert(DeliverOrder objOrd)
        {
            _db.DeliverOrders.Add(objOrd);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(DeliverOrder ord)
        {
            _db.Entry(ord).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
