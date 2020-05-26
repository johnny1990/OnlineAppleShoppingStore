using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineAppleShoppingStore.Entities.Models;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IFeedbackRepository : IDisposable
    {
        IQueryable<Feedback> All { get; }
        void Insert(Feedback f);
        void Save();
    }
}
