using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        // Contract for Specifications
        public Expression<Func<T,bool>>? Criteria { get; set; } // where condition : E => E.id == id
        public List<Expression<Func<T, object>>> Includes { get; set; } // List of includes : E => E.Brand

        public Expression<Func<T , object>> OrderBy { get; set; } // p => p.Name
        public Expression<Func<T, object>> OrderByDesc { get; set; } // p => p.price
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
