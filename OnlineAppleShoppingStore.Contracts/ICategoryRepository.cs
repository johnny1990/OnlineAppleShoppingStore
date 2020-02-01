using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface ICategoryRepository : IDisposable
    {
        IQueryable<Category> All { get; }
        Category Find(int? id);
        void Insert(Category cat);
        void Update(Category cat);
        void Delete(int id);
        void Save();
    }
}
