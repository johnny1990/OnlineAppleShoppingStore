using OnlineAppleShoppingStore.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IDeliverOrdersRepository : IDisposable
    {
        IQueryable<DeliverOrder> All { get; }
        DbQuery<DeliverOrder> Alls { get; }
        DeliverOrder Find(int? id);
        void Insert(DeliverOrder ord);
        void Update(DeliverOrder ord);
        void Save();
    }
}
