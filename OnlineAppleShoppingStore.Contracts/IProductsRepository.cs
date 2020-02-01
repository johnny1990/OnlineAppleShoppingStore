using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IProductsRepository : IDisposable
    {
        IQueryable<Product> All { get; }
        Product Find(int? id);
        void Insert(Product prod);
        void Update(Product prod);
        void Delete(int id);
        void Save();
    }
}
