using Infrastructure.Dtos;

namespace Application.Services;

public interface ICryptoCurrencyService
{
    Task<List<RateResponse>> GetExchangeRatesAsync(RateRequest rateRequest, string[] fiats);
}