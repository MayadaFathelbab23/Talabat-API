using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.APIs.Helper;
using Talabat.Core;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.Product_Specifications;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : APIBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork
                                , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // Get All Products : /api/Products
        // Get All Products : /api/Products?sort=priceAsc
        // Get All Products : /api/Products?sort=priceDesc&brandId=1&typeId=2
        // Get All Products : /api/Products?pageSize=5&pageIndex=2
        // Get All Products : /api/Products?search=mocha
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productSpecParams)
        {
            var spec = new ProductWithBrandAndTypeSpec(productSpecParams);
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var mappedProducts = _mapper.Map<IReadOnlyList<Product> , IReadOnlyList<ProductToReturnDto>>(products);
            var countSpec = new ProductsWithFilterationSpec(productSpecParams);
            var count = await _unitOfWork.Repository<Product>().GetCount(countSpec);
            var productsPagination = new Pagination<ProductToReturnDto>(productSpecParams.PageSize , productSpecParams.PageIndex , mappedProducts , count);
            return Ok(productsPagination);
        }

        // Get Product By Id : /api/products/id
        [ProducesResponseType(typeof(ProductToReturnDto) , StatusCodes.Status200OK)] // improve swagger Doc.
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithBrandAndTypeSpec(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if(product == null) 
                return NotFound(new ApiResponse(404));
            var mappedProduct = _mapper.Map<Product , ProductToReturnDto>(product);
            return Ok(mappedProduct);
        }

        [HttpGet("brands")]  // GET : /api/Products/brands
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrands()
        {
            return await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
        }
        [HttpGet("types")] // GET : /api/products/types
        public async Task<IReadOnlyList<ProductType>> GetProductTypes()
        {
            return await _unitOfWork.Repository<ProductType>().GetAllAsync();
        }
    }
}
