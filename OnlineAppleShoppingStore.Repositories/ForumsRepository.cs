using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class ForumsRepository : IForumsRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public ForumsRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<Forum> All
        {
            get { return _db.Fora; }
        }

        public void Delete(int id)
        {
            var f = _db.Fora.Find(id);
            _db.Fora.Remove(f);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Forum Find(int? id)
        {
            Forum obj = new Forum();
            obj = _db.Fora.Where(p => p.Id == id).FirstOrDefault();
            return obj;
        }

        public void Insert(Forum f)
        {
            _db.Fora.Add(f);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Forum f)
        {
            _db.Entry(f).State = System.Data.Entity.EntityState.Modified;

        }
    }
}
