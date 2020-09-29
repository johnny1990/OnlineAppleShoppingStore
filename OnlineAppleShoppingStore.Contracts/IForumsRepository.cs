using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IForumsRepository : IDisposable
    {
        IQueryable<Forum> All { get; }
        Forum Find(int? id);
        void Insert(Forum f);
        void Update(Forum f);
        void Delete(int id);
        void Save();
    }
}
