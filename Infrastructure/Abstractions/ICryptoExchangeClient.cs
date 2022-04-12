using Infrastructure.Models.CoinMarketCap;

namespace Infrastructure.Abstractions;

public interface ICryptoExchangeClient
{
    Task<CoinMarketCapResponseModel<CryptoCurrency>> GetCryptoQuoteAsync(string symbol,
        string fiat);

    Task<CoinMarketCapResponseModel<CryptoCurrencyMetadata>> GetCryptoCurrencyMetadataAsync(string symbol);
}