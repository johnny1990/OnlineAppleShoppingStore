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
    public class CategoryRepository : ICategoryRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public CategoryRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<Category> All
        {
            get { return _db.Categories; }
        }
       
        public DbQuery<Category> Alls
        {
            get { return _db.Categories; }
        }

        public void Delete(int id)
        {
            var cat = _db.Categories.Find(id);
            _db.Categories.Remove(cat);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Category Find(int? id)
        {
            Category objCat = new Category();
            objCat = _db.Categories.Where(p => p.Id == id).FirstOrDefault();
            return objCat;
        }

        public void Insert(Category cat)
        {
            _db.Categories.Add(cat);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Category cat)
        {
            _db.Entry(cat).State = System.Data.Entity.EntityState.Modified;

        }
    }
}
