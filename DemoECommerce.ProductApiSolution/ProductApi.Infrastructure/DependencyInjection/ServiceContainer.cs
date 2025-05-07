using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity
            // Add authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection (DI)
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }
        public static IApplicationBuilder UseInfrastrcturePolicy(this  IApplicationBuilder app) 
        {
            //register middleware such as:
            //global exception:handles external errors
            //listen to only Api Gateway:blocks all outsider calls;
            SharedServiceContainer.UseSharedPolicies(app);
            return app;

           }
    }
}
