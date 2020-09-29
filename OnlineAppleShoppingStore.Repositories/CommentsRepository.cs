using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public CommentsRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }

        public IQueryable<Comment> All
        {
            get { return _db.Comments; }
        }

        public void Delete(int id)
        {
            var f = _db.Comments.Find(id);
            _db.Comments.Remove(f);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Comment Find(int? id)
        {
            Comment objC = new Comment();
            objC = _db.Comments.Where(p => p.Id == id).FirstOrDefault();
            return objC;
        }

        public void Insert(Comment ct)
        {
            _db.Comments.Add(ct);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Comment ct)
        {
            _db.Entry(ct).State = System.Data.Entity.EntityState.Modified;

        }
    }
}
