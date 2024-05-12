using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        // Complete => SaveChanges
        Task<int> CompleteAsync();

        // Repository => Dbset
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
    }

}
