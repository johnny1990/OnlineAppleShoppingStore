using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        AuthEntities _db;

        public UsersRepository(AuthEntities db)
        {
            this._db = db;
        }

        public IQueryable<AspNetUser> All
        {
            get { return _db.AspNetUsers; }
        }

        public void Delete(int id)
        {
            var c = _db.AspNetUsers.Find(id);
            _db.AspNetUsers.Remove(c);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Insert(AspNetUser user)
        {
            _db.AspNetUsers.Add(user);
        }

        public void Update(AspNetUser user)
        {
            _db.Entry(user).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            _db.SaveChanges();
        }    
    }
}
