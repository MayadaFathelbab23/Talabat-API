using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            // Seed Brands
            if(!context.ProductBrands.Any())
            {
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json"); // read data as text
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                        await context.Set<ProductBrand>().AddAsync(Brand);
                    await context.SaveChangesAsync();
                }
            }

            // Seed Types
           if (!context.ProductTypes.Any())
            {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                        await context.Set<ProductType>().AddAsync(Type);
                    await context.SaveChangesAsync();
                }
            }

            // Seed Products
           if(!context.Products.Any())
            {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                        await context.Set<Product>().AddAsync(Product);
                    await context.SaveChangesAsync();
                }
            }

            // Seed Delivery Methods
            if (!context.DeliveryMethods.Any())
            {
                var DeliverMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliverMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliverMethodsData);
                if (DeliverMethods?.Count > 0)
                {
                    foreach (var DeliveryMethod in DeliverMethods)
                        await context.Set<DeliveryMethod>().AddAsync(DeliveryMethod);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
