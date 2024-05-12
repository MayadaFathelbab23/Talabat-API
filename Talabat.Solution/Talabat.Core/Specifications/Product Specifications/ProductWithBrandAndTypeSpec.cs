using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications.Product_Specifications
{
    public class ProductWithBrandAndTypeSpec : BaseSpecifications<Product>
    {
        // ctor to get all products and include Brand and type 
        public ProductWithBrandAndTypeSpec(ProductSpecParams productSpecParams) 
            : base(
                    p => 
                    (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search)) &&
                    (!productSpecParams.BrandId.HasValue || p.ProductBrandId == productSpecParams.BrandId.Value) &&
                    (!productSpecParams.TypeId.HasValue || p.ProductTypeId == productSpecParams.TypeId.Value)
                  )
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            // based on sort value [sort=PriceAsc , PriceDesc , Name]
            if (!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case "priceAsc":
                        OrderBy = p => p.Price;
                        break;
                    case "priceDesc":
                        OrderByDesc = p => p.Price;
                        break;
                    case "name":
                        OrderBy = p => p.Name;
                        break;
                    default:
                        OrderBy = p => p.Name;
                        break;
                }
            }
            else
                OrderBy = p => p.Name;
            AppluPagination((productSpecParams.PageIndex - 1) * productSpecParams.PageSize, productSpecParams.PageSize);
        }
        public ProductWithBrandAndTypeSpec(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
        
    }
}
