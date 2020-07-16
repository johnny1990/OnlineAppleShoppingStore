using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IOrdersRepository : IDisposable
    {
        IQueryable<Order> All { get; }
        DbQuery<Order> Alls { get; }
    }
}
