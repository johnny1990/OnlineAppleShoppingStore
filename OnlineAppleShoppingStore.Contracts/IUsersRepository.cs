using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IUsersRepository : IDisposable
    {
        IQueryable<AspNetUser> All { get; }
        void Insert(AspNetUser user);
        void Update(AspNetUser user);
        void Delete(int id);
        void Save();
    }
}
