using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        AuthEntities _db;

        public UserRolesRepository(AuthEntities db)
        {
            this._db = db;
        }

        public IQueryable<AspNetUserRole> All
        { 
           get { return _db.AspNetUserRoles; }
        }

        public void Delete(int id)
        {
            var c = _db.AspNetUserRoles.Find(id);
            _db.AspNetUserRoles.Remove(c);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public AspNetUserRole Find(int? id)
        {
            AspNetUserRole role = new AspNetUserRole();
            role = _db.AspNetUserRoles.Where(p => p.Id == id).FirstOrDefault();
            return role;
        }

        public void Insert(AspNetUserRole role)
        {
            _db.AspNetUserRoles.Add(role);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(AspNetUserRole role)
        {
            _db.Entry(role).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
