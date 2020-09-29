using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface ICommentsRepository : IDisposable
    {
        IQueryable<Comment> All { get; }
        Comment Find(int? id);
        void Insert(Comment ct);
        void Update(Comment ct);
        void Delete(int id);
        void Save();
    }
}
