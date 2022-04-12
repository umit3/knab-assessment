using Newtonsoft.Json;

namespace Infrastructure.Models.CoinMarketCap;

public class CryptoCurrencyPrice
{
    [JsonProperty("price")]
    public double Value { get; set; }
}