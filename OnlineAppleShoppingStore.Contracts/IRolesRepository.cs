using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IRolesRepository :IDisposable
    {
        IQueryable<AspNetRole> All { get; }
        AspNetRole Find(string id);
        void Insert(AspNetRole user);
        void Update(AspNetRole user);
        void Delete(int id);
        void Save();
    }
}
