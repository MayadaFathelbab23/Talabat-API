using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Error;
using Talabat.Core.Models;
using Talabat.Core.Repositories;

namespace Talabat.APIs.Controllers
{
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet] // GET or ReCreate basket : /api/basket?id=
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket??  new CustomerBasket(id));
        }

        [HttpPost] // POST : /api/basket
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasketDto , CustomerBasket>(basketDto);
            var UpdatedOrCreatedBasket = await _basketRepository.UpdateBasketAsync(basket);
            if (UpdatedOrCreatedBasket != null)
                return Ok(UpdatedOrCreatedBasket);
            else
                return BadRequest(new ApiResponse(400));
        }

        [HttpDelete] // /api/basket?id=
        public async Task DeleteBasket(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}
