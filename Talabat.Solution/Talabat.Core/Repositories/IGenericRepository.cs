using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // Signature for 5 methods
        // 1. Get All
        Task<IReadOnlyList<T>> GetAllAsync();
        // 2. Get By Id
        Task<T?> GetByIdAsync(int id);

        // 3. Add
        Task AddAsync(T entity);

        // Get All using Specification
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        // Get by id using Specification
        Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec);

        // Get Count
        Task<int> GetCount(ISpecifications<T> spec);

        void Delete(T entity);
        void Update(T entity);
    }
}
