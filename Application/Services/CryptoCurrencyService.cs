using Infrastructure.Abstractions;
using Infrastructure.Dtos;
using Infrastructure.Models.CoinMarketCap;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class CryptoCurrencyService : ICryptoCurrencyService
{
    private readonly ICryptoExchangeClient _cryptoExchangeClient;

    public CryptoCurrencyService(
        ICryptoExchangeClient cryptoExchangeClient)
    {
        _cryptoExchangeClient = cryptoExchangeClient;
    }

    public async Task<List<RateResponse>> GetExchangeRatesAsync(RateRequest rateRequest, string[] fiats)
    {
        var responseTaskList = new List<Task<CoinMarketCapResponseModel<CryptoCurrency>>>();

        foreach (var fiat in fiats)
        {
            var responseTask = _cryptoExchangeClient.GetCryptoQuoteAsync(rateRequest.Symbol, fiat);
            responseTaskList.Add(responseTask);
        }

        var responses = await Task.WhenAll(responseTaskList);

        return responses.Select(response => new RateResponse
        {
            Succeeded = response.Succeeded,
            ErrorMessage = response.Status!.ErrorMessage,
            CryptoCurrency = response.Data?.First().Value.First(),
        }).ToList();
    }
}