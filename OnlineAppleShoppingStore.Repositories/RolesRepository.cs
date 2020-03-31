using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        AuthEntities _db;

        public RolesRepository(AuthEntities db)
        {
            this._db = db;
        }

        public IQueryable<AspNetRole> All
        {
            get { return _db.AspNetRoles; }
        }

        public void Delete(int id)
        {
            var c = _db.AspNetRoles.Find(id);
            _db.AspNetRoles.Remove(c);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Insert(AspNetRole role)
        {
            _db.AspNetRoles.Add(role);
        }

        public void Update(AspNetRole role)
        {
            _db.Entry(role).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public AspNetRole Find(string id)
        {
            AspNetRole role = new AspNetRole();
            role = _db.AspNetRoles.Where(p => p.Id == id).FirstOrDefault();
            return role;
        }
    }
}
