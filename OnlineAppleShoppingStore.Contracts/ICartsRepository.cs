using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface ICartsRepository : IDisposable
    {
        IQueryable<Cart> All { get; }
        void Insert(Cart cart);
        void Delete(int id);
        void Save();
    }
}
