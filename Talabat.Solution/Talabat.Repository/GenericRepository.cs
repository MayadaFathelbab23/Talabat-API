using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        #region Without Specification
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            // In product -> need to include Brand and type [Eager loading]
            //if(typeof(T) == typeof(Product))
            //{
            //    return (IEnumerable<T>) await _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            //}
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }


        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        #endregion
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpec(spec).AsNoTracking().ToListAsync(); 
        }

        public async Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpec(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCount(ISpecifications<T> spec)
        {
            return await ApplySpec(spec).CountAsync();
        }
         private IQueryable<T> ApplySpec(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        =>  _context.Set<T>().Remove(entity);

        public void Update(T entity)
        =>  _context.Set<T>().Update(entity);
    }
}
