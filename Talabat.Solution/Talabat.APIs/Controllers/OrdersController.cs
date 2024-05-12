using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.IdentityDtos;
using Talabat.APIs.Error;
using Talabat.Core.Models.Order_Aggregate;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService , IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        // Create order
        [HttpPost] // POST : /api/orders
        [Authorize]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email); // get email from user claims
            var mappedAddress = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);
            var order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, mappedAddress, orderDto.DeliveryMethodId);
            return order is null ? BadRequest(new ApiResponse(400 , "Create Order Faild")) : Ok(_mapper.Map<Order , OrderToReturnDto>(order));
        }

        // Get Orders For User
        [HttpGet] // GET : /api/orders
        [Authorize]
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]  
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]  
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForSpecificUserAsync(email);
            if (orders is null) return NotFound(new ApiResponse(404, "No Orders Exists"));
            var mappedOrders = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList< OrderToReturnDto >> (orders);
            return Ok(mappedOrders);
        }

        // Get Order By Id for User
        [HttpGet("{id}")] // GET : /api/orders/id
        [Authorize]
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);   
            var order = await _orderService.GetOrderByIdForSpecificUserAsync(email , id);
            if (order is null) return NotFound(new ApiResponse(404, "No Order Exists for this id"));
            var mappedOrder = _mapper.Map<Order, OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }

        // Get All deliveryMethods
        [HttpGet("DeliveryMethod")] // GET : /api/orders/DeliveryMethod
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetAllDeliveryMethods()
        {
            var deliverMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(deliverMethods);
        }
    }
}
