using Newtonsoft.Json;

namespace Infrastructure.Models.CoinMarketCap;

public class CryptoCurrency : CryptoCurrencyMetadata
{
    [JsonProperty("quote")]
    public Dictionary<string, CryptoCurrencyPrice> Quote { get; set; }
}