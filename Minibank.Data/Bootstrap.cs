﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minibank.Core;
using Minibank.Core.Domains.BankAccounts.Repositories;
using Minibank.Core.Domains.Users.Repositories;
using Minibank.Data.BankAccounts.Repositories;
using Minibank.Data.Users.Repositories;
using System;

namespace Minibank.Data
{
    public static class Bootstrap
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IExchangeRates, ExchangeRates>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();

            services.AddHttpClient<IExchangeRates, ExchangeRates>(options => {
                options.BaseAddress = new Uri(configuration["ExchangeRatesUri"]);
            });

            return services;
        }
    }
}
