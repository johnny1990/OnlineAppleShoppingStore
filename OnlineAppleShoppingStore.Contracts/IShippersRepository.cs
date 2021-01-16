using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IShippersRepository: IDisposable
    {
        IQueryable<ShippersOrder> All { get; }
        ShippersOrder Find(int? id);
        void Insert(ShippersOrder so);
        void Update(ShippersOrder so);
        void Delete(int id);
        void Save();
    }
}
