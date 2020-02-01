using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public ProductsRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<Product> All
        {
            get { return _db.Products; }
        }

        public void Delete(int id)
        {
            var prod = _db.Products.Find(id);
            _db.Products.Remove(prod);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Product Find(int? id)
        {
            Product objProd = new Product();
            objProd = _db.Products.Where(p => p.Id == id).FirstOrDefault();
            return objProd;
        }

        public void Insert(Product prod)
        {
            _db.Products.Add(prod);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Product prod)
        {
            _db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
