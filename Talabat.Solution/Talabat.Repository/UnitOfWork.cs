using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private readonly Hashtable _repoHashTable;
        public UnitOfWork(StoreContext storeContext)
        {
            _storeContext = storeContext;
            _repoHashTable = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        => await _storeContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        => await _storeContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var keyType = typeof(TEntity).Name; // hashtable key
            if (!_repoHashTable.ContainsKey(keyType))
            {
                var Repository = new GenericRepository<TEntity>(_storeContext);
                _repoHashTable.Add(keyType, Repository);
            }
            return _repoHashTable[keyType] as IGenericRepository<TEntity>;
        }
    }

}
