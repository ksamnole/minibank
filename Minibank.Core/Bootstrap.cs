using Microsoft.Extensions.DependencyInjection;

namespace Minibank.Core
{
    public static class Bootstrap
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ICurrencyConverter, CurrencyConverter>();

            return services;
        }
    }   
}
