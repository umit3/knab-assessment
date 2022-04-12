using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<ICryptoExchangeClient, CoinMarketCapClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("CoinMarketCap:BaseUrl"));
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", configuration.GetValue<string>("CoinMarketCap:ApiKey"));
        });
        return services;
    }
    
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidation();
        return services;
    }
}