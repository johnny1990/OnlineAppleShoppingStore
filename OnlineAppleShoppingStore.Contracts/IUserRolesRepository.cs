using OnlineAppleShoppingStoreAuth.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineAppleShoppingStore.Contracts
{
    public interface IUserRolesRepository : IDisposable
    {
        IQueryable<AspNetUserRole> All { get; }
        AspNetUserRole Find(int? id);
        void Insert(AspNetUserRole role);
        void Update(AspNetUserRole role);
        void Delete(int id);
        void Save();
}
}
