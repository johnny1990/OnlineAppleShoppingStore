using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface ICustomersRepository :IDisposable
    {
        IQueryable<Customer> All { get; }
        DbQuery<Customer> Alls { get; }
        Customer Find(int? id);
        void Insert(Customer ct);
        void Update(Customer ct);
        void Delete(int id);
        void Save();
    }
}
