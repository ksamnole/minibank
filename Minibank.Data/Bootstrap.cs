using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core;
using System;

namespace Minibank.Data
{
    public static class Bootstrap
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExchangeRates, ExchangeRates>();

            services.AddHttpClient<IExchangeRates, ExchangeRates>(options => {
                options.BaseAddress = new Uri(configuration["ExchangeRatesUri"]);
            });

            return services;
        }
    }
}
