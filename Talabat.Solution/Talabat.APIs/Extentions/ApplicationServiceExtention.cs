using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Error;
using Talabat.APIs.Helper;
using Talabat.Core;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extentions
{
    public static class ApplicationServiceExtention
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            // Allow DI to GenericRepository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Allow DI to BasketRepository
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            // Allow DI to IMapper 
            services.AddAutoMapper(typeof(MappingProfiles));
            // configure ApiBehaviorOptions to customize how validation errors are handled:
            services.Configure<ApiBehaviorOptions>(optins =>
            {
                // options : instance of ApiBehaviorOptions being configured.
                // InvalidModelStateResponseFactory : This property allows you to customize the response returned when model state validation fails
                // actioContext : Represents the current ActionContext. that contains information about the current HTTP request,
                // including the model state, action arguments, route data, and other contextual information.
                optins.InvalidModelStateResponseFactory = (actioContext) =>
                {
                    var errors = actioContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(E => E.ErrorMessage)
                                                        .ToList();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });

            services.AddScoped(typeof(IUnitOfWork) , typeof(UnitOfWork));
            services.AddScoped(typeof(IOrderService) , typeof(OrderService));
            services.AddScoped(typeof(IPaymentService) , typeof(PaymentService));

            return services;
        }
    }
}
