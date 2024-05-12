using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> startQuery , ISpecifications<T> spec)
        {
            var query = startQuery; // _dbContext.Set<T>()
            // 1. where condition
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria); //  _dbContext.Set<T>().where(E => E.id == id)

            // 2. Sort
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);
            // 3. Skip - Take
            if(spec.IsPaginationEnabled )
                query = query.Skip(spec.Skip).Take(spec.Take);
            // 4. list of includes [ Aggregate - Accumlation]
            query = spec.Includes.Aggregate(query, (CurrentQuery, includeExpression) => CurrentQuery.Include(includeExpression));
            // _dbContext.Set<T>().where(E => E.id == id).include(E => E.Brand)
            // _dbContext.Set<T>().where(E => E.id == id).include(E => E.Brand).include(E => E.Type)
            return query;
        }
    }
}
