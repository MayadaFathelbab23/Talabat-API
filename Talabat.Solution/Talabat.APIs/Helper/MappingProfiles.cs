using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.APIs.DTOs.IdentityDtos;
using Talabat.Core.Models;
using Talabat.Core.Models.Identity;
using Talabat.Core.Models.Order_Aggregate;
using OrderAddress = Talabat.Core.Models.Order_Aggregate.Address;
using IdentityAddress = Talabat.Core.Models.Identity.Address;

namespace Talabat.APIs.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product , ProductToReturnDto>()
                .ForMember( d => d.ProductBrand ,  o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType , o => o.MapFrom( s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl , o => o.MapFrom<ProductPictureUrlResolver>());
            // Generic MapFrom<IValueResolver>

            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<IdentityAddress, AddressDto>().ReverseMap();
            CreateMap< AddressDto , OrderAddress>();

            // Return Order mapping
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost))
                .ForMember(d => d.Total , o => o.MapFrom(s => s.GetTotal()));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
                
        }
    }
}
