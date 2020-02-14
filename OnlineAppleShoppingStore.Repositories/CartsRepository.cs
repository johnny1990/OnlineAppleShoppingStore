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

        public void Delete(int id)
        {
            var c = _db.Categories.Find(id);
            _db.Categories.Remove(c);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Insert(Cart cart)
        {
            _db.Carts.Add(cart);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
