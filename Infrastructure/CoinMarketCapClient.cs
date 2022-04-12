using Infrastructure.Abstractions;
using Infrastructure.Models.CoinMarketCap;
using Newtonsoft.Json;

namespace Infrastructure;

public class CoinMarketCapClient : ICryptoExchangeClient
{
    private readonly HttpClient _httpClient;

    public CoinMarketCapClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CoinMarketCapResponseModel<CryptoCurrency>> GetCryptoQuoteAsync(string symbol, string fiat)
    {
        var exchangeApiResponse =
            await _httpClient.GetAsync($"/v2/cryptocurrency/quotes/latest?symbol={symbol}&convert={fiat}");
        
        var responseString = await exchangeApiResponse.Content.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(responseString))
        {
            var response = JsonConvert.DeserializeObject<CoinMarketCapResponseModel<CryptoCurrency>>(responseString);
            return response;
        }

        return null;
    }

    public async Task<CoinMarketCapResponseModel<CryptoCurrencyMetadata>> GetCryptoCurrencyMetadataAsync(string symbol)
    {
        var exchangeApiResponse =
            await _httpClient.GetAsync($"/v2/cryptocurrency/info?symbol={symbol}");
        
        var responseString = await exchangeApiResponse.Content.ReadAsStringAsync();
        if (!string.IsNullOrEmpty(responseString))
        {
            var response = JsonConvert.DeserializeObject<CoinMarketCapResponseModel<CryptoCurrencyMetadata>>(responseString);
            return response;
        }

        return null;
    }
}