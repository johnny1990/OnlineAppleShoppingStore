using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public CustomersRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<Customer> All
        {
            get { return _db.Customers; }
        }

        public DbQuery<Customer> Alls
        {
            get { return _db.Customers; }
        }

        public void Delete(int id)
        {
            var cat = _db.Customers.Find(id);
            _db.Customers.Remove(cat);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Customer Find(int? id)
        {
            Customer objCt = new Customer();
            objCt = _db.Customers.Where(p => p.Id == id).FirstOrDefault();
            return objCt;
        }

        public void Insert(Customer ct)
        {
            _db.Customers.Add(ct);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Customer ct)
        {
            _db.Entry(ct).State = System.Data.Entity.EntityState.Modified;

        }
    }
}
