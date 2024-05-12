using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>>? Criteria { get ; set; }
        public List<Expression<Func<T, object>>> Includes { get ; set ; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get ; set; }
        public Expression<Func<T, object>> OrderByDesc { get ; set ; }
        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPaginationEnabled { get; set ; }

        // ctor to get all => has no criteria - where condition
        public BaseSpecifications()
        {
            // criteria = null
            
        }
        // ctor to get by id => contain criteria - where condition
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
            //Includes = new List<Expression<Func<T, object>>>();
        }
        public void AddOrderByDesc(Expression<Func<T, object>> exp)
        {
            OrderByDesc = exp;
        }public void AddOrderBy(Expression<Func<T, object>> exp)
        {
            OrderBy = exp;
        }
        public void AppluPagination(int skip , int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}
