using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineAppleShoppingStore.Contracts;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        OnlineAppleShoppingStoreEntities _db;

        public FeedbackRepository(OnlineAppleShoppingStoreEntities db)
        {
            this._db = db;
        }
        public IQueryable<Feedback> All
        {
            get { return _db.Feedbacks; }
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Insert(Feedback f)
        {
            _db.Feedbacks.Add(f);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
