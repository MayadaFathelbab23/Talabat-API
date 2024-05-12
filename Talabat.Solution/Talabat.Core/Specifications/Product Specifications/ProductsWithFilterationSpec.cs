using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications.Product_Specifications
{
    public class ProductsWithFilterationSpec : BaseSpecifications<Product>
    {
        public ProductsWithFilterationSpec(ProductSpecParams productSpecParams) 
            : base(p => 
                    (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search)) &&
                    (!productSpecParams.BrandId.HasValue || p.ProductBrandId == productSpecParams.BrandId) &&
                    (!productSpecParams.TypeId.HasValue || p.ProductTypeId == productSpecParams.TypeId)
            )
        {
            
        }
    }
}
